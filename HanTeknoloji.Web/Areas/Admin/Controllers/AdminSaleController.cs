using HanTeknoloji.Data.Models.Orm.Entity;
using HanTeknoloji.Web.Areas.Admin.Models.Types.Enums;
using HanTeknoloji.Web.Areas.Admin.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HanTeknoloji.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class AdminSaleController : AdminBaseController
    {
        public ActionResult Index()
        {
            CartVM model = new CartVM();
            model.ProductList = new List<ProductVM>();
            if (Session["Sepet"] != null)
            {
                model = (CartVM)Session["Sepet"];
            }
            GetCustomers();
            return View(model);
        }

        private void GetCustomers()
        {
            ViewData["customer"] = rpcustomer.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name + " Tel:" + x.Phone,
                Value = x.ID.ToString()
            }).ToList();
        }

        [HttpPost]
        public ActionResult Index(string BarcodeNumber)
        {
            CartVM model = new CartVM();
            model.ProductList = new List<ProductVM>();
            if (!string.IsNullOrEmpty(BarcodeNumber))
            {
                var product = rpproduct.FirstOrDefault(x => x.SerialNumber == BarcodeNumber);
                if (product != null)
                {
                    if (product.Count != 0)
                    {
                        product.Count -= 1;
                        rpproduct.SaveChanges();

                        bool varMi = false;

                        if (Session["Sepet"] != null)
                        {
                            model = (CartVM)Session["Sepet"];
                            foreach (var item in model.ProductList)
                            {
                                if (item.SerialNumber == BarcodeNumber)
                                {
                                    item.SaleCount++;
                                    item.Count = product.Count;
                                    varMi = true;
                                    item.UnitSalePrice = item.UnitPrice * item.SaleCount;
                                    item.KdvPrice = Math.Round(item.UnitSalePrice * item.KDV, 4);
                                    item.TotalPrice = 0;
                                    model.TotalSalePrice += item.TotalPrice / item.SaleCount;
                                    break;
                                }
                            }
                        }

                        if (!varMi)
                        {
                            product = rpproduct.FirstOrDefault(x => x.SerialNumber == BarcodeNumber);
                            ProductVM pro = new ProductVM()
                            {
                                ID = product.ID,
                                SerialNumber = product.SerialNumber,
                                TradeMark = rptrademark.Find(product.TradeMarkID).Name,
                                ProductModel = product.ProductModelID == 0 ? "" : rpproductmodel.Find(product.ProductModelID).Name,
                                Color = rpcolor.Find(product.ColorID).Name,
                                UnitSalePrice = product.UnitSalePrice,
                                UnitPrice = product.UnitSalePrice,
                                Count = product.Count,
                                SaleCount = 1,
                                KDV = product.KDV,
                                KdvPrice = Math.Round((product.UnitSalePrice * product.KDV), 4)
                            };
                            model.ProductList.Add(pro);
                            model.TotalSalePrice += pro.TotalPrice;
                        }
                        Session["Sepet"] = model;
                    }
                    else
                    {
                        if (Session["Sepet"] != null)
                        {
                            model = (CartVM)Session["Sepet"];
                        }
                        ViewBag.IslemDurum = EnumIslemDurum.StokYetersiz;
                        GetCustomers();
                        return View(model);
                    }
                }
                else
                {
                    if (Session["Sepet"] != null)
                    {
                        model = (CartVM)Session["Sepet"];
                    }
                    ViewBag.IslemDurum = EnumIslemDurum.UrunYok;
                    GetCustomers();
                    return View(model);
                }
            }
            GetCustomers();
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var product = rpproduct.Find(id);
            var sepet = (CartVM)Session["Sepet"];
            var selectedItem = sepet.ProductList.FirstOrDefault(x => x.ID == id);
            sepet.TotalSalePrice -= selectedItem.TotalPrice;

            product.Count += selectedItem.SaleCount;
            rpproduct.SaveChanges();

            sepet.ProductList.Remove(selectedItem);
            Session["Sepet"] = sepet;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeQuantity(int id, int quantity)
        {
            var product = rpproduct.Find(id);
            var sepet = (CartVM)Session["Sepet"];
            var selectedItem = sepet.ProductList.FirstOrDefault(x => x.ID == id);

            int oldCount = product.Count + selectedItem.SaleCount;
            product.Count = oldCount - quantity;
            rpproduct.SaveChanges();

            selectedItem.Count = product.Count;
            selectedItem.SaleCount = quantity;
            selectedItem.UnitSalePrice = selectedItem.UnitPrice * selectedItem.SaleCount;
            selectedItem.KdvPrice = Math.Round(selectedItem.UnitSalePrice * selectedItem.KDV, 4);
            selectedItem.TotalPrice = 0;
            sepet.TotalSalePrice = 0;
            foreach (var item in sepet.ProductList)
            {
                sepet.TotalSalePrice += item.TotalPrice;
            }

            Session["Sepet"] = sepet;
            return RedirectToAction("Index");
        }

        public JsonResult ChangeProductSalePrice(ChangePriceVM model)
        {
            if (model.Price != 0)
            {
                var sepet = (CartVM)Session["Sepet"];
                var selectedItem = sepet.ProductList.FirstOrDefault(x => x.ID == model.ID);

                sepet.TotalSalePrice -= selectedItem.TotalPrice;
                selectedItem.TotalPrice = model.Price;
                selectedItem.UnitPrice = Math.Round((model.Price / (selectedItem.KDV + 1)) / selectedItem.SaleCount, 4);
                selectedItem.UnitSalePrice = selectedItem.UnitPrice * selectedItem.SaleCount;
                sepet.TotalSalePrice += selectedItem.TotalPrice;
                selectedItem.KdvPrice = Math.Round(selectedItem.TotalPrice - selectedItem.UnitSalePrice, 4);
                return Json("succ");
            }
            else
            {
                return Json("fail");
            }

        }

        [HttpPost]
        public ActionResult AddSale(SaleVM model)
        {
            var sepet = (CartVM)Session["Sepet"];
            if (sepet != null)
            {
                int userId = UserID();
                model.InvoiceDate = model.InvoiceDate.Year == 0001 ? DateTime.Now : model.InvoiceDate;
                foreach (var item in sepet.ProductList)
                {
                    Sale entity = new Sale()
                    {
                        ProductID = item.ID,
                        PaymentType = model.PaymentType,
                        Price = item.TotalPrice,
                        Quantity = item.SaleCount,
                        UserID = userId,
                        CustomerID = model.CustomerID,
                        InvoiceDate = model.InvoiceDate,
                        KdvPrice = item.KdvPrice
                    };
                    rpsale.Add(entity);
                }
                if (model.Invoice == 1)
                {
                    return RedirectToAction("SetInvoice", model);
                }
                Session.Remove("Sepet");
            }
            return RedirectToAction("Index");
        }

        public ActionResult SetInvoice(SaleVM saleVM)
        {
            var sepet = (CartVM)Session["Sepet"];
            var customer = rpcustomer.Find(saleVM.CustomerID);
            InvoiceVM model = new InvoiceVM();
            model.CustomerName = customer.Name;
            model.Address = customer.Address;
            model.City = rpcity.Find(customer.CityID).Name;
            model.Region = rpregion.Find(customer.RegionID).Name;
            model.TaxOffice = customer.TaxOffice;
            model.TaxNumber = customer.TaxNumber;
            model.ProductList = sepet.ProductList;
            model.TotalSalePrice = sepet.TotalSalePrice;
            model.PriceString = saleVM.PriceString;
            model.TakerName = saleVM.Name;

            model.DateOfArrangement = String.Format("{0:d/M/yyyy}", saleVM.InvoiceDate);
            model.HourOfArrangement = String.Format("{0:HH:mm}", saleVM.InvoiceDate);

            decimal kdv = 0;
            model.KDVList = new List<decimal>();
            model.KDVStringList = new List<string>();
            model.KDVList.Add(model.TotalUnitPrice);
            model.KDVStringList.Add("TOPLAM");
            foreach (var item in model.ProductList)
            {
                if (item.KDV != kdv)
                {
                    model.KDVList.Add(Math.Round(model.ProductList.Where(x => x.KDV == item.KDV).Sum(x => x.KdvPrice), 2));
                    model.KDVStringList.Add("KDV %" + Convert.ToInt32(item.KDV * 100));
                }
                kdv = item.KDV;
            }
            model.KDVList.Add(model.TotalSalePrice);
            model.KDVStringList.Add("G.TOPLAM");
            Session.Remove("Sepet");
            return View(model);
        }

        [HttpPost]
        public ActionResult AddServiceSale(ServiceSaleVM model)
        {
            if (ModelState.IsValid)
            {
                int userId = UserID();
                ServiceSale entity = new ServiceSale()
                {
                    PaymentType = model.PaymentType,
                    Price = model.Price,
                    UserID = userId,
                    Note = model.Note
                };
                rpservicesale.Add(entity);
            }
            ViewBag.IslemDurum = EnumIslemDurum.Basarili;
            return RedirectToAction("Index");
        }
    }
}