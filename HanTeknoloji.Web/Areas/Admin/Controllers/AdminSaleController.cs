using HanTeknoloji.Data.Models.Orm.Entity;
using HanTeknoloji.Web.Areas.Admin.Models.Services;
using HanTeknoloji.Web.Areas.Admin.Models.Types.Enums;
using HanTeknoloji.Web.Areas.Admin.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HanTeknoloji.Web.Areas.Admin.Controllers
{
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
                var product = rpproduct.FirstOrDefault(x => x.SerialNumber == BarcodeNumber && x.Count != 0);
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
                            ProductVM pro = new ProductVM()
                            {
                                ID = product.ID,
                                SerialNumber = product.SerialNumber,
                                TradeMark = rptrademark.Find(product.TradeMarkID).Name,
                                ProductModel = product.ProductModelID == 0 ? "" : rpproductmodel.Find(product.ProductModelID).Name,
                                Color = rpcolor.Find(product.ColorID).Name,
                                UnitSalePrice = product.UnitSalePrice,//satış fiyatı değişken
                                UnitPrice = product.UnitSalePrice,//satış fiyatı
                                UnitBuyPrice = product.UnitPrice,//alış fiyatı
                                Count = product.Count,
                                SaleCount = 1,
                                KDV = product.KDV,
                                PaymentInfoID = rppaymentinfo.GetListWithQuery(x => x.ProductID == product.ID).Last().ID,
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
                Sale sale = new Sale()
                {
                    PaymentType = model.PaymentType,
                    UserID = userId,
                    CustomerID = model.CustomerID,
                    InvoiceDate = model.InvoiceDate,
                    ExpiryDate = model.ExpiryDate.Year == 0001 ? DateTime.Now : model.ExpiryDate,
                    TotalPrice = sepet.TotalSalePrice,
                    IsInvoiced = model.Invoice == 1 ? true : false
                };
                var list = new List<SaleDetails>();
                foreach (var item in sepet.ProductList)
                {
                    SaleDetails detail = new SaleDetails()
                    {
                        ProductID = item.ID,
                        KdvPrice = Math.Round(item.KdvPrice, 2),
                        Quantity = item.SaleCount,
                        Price = Math.Round(item.UnitSalePrice, 2),
                        AddDate = DateTime.Now,
                        UnitBuyPrice = item.UnitBuyPrice,
                        UnitSalePrice = item.UnitSalePrice,
                        PaymentInfoID = item.PaymentInfoID
                    };
                    list.Add(detail);
                }
                sale.SaleDetails = list;
                rpsale.Add(sale);
                if (model.PaymentType == "Vadeli")
                {
                    CustomerExpiry expiry = new CustomerExpiry()
                    {
                        ExpiryDate = model.ExpiryDate,
                        CustomerID = model.CustomerID,
                        ExpiryValue = sepet.TotalSalePrice - model.PaidExpiryValue,
                        SaleID = sale.ID,
                        SaleTotalPrice = sepet.TotalSalePrice
                    };
                    ExpiryService.SetCustomerExpiry(expiry);
                    rpcustomerexpiry.Add(expiry);
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
            model.TaxOffice = customer.IsPerson ? "" : customer.TaxOffice;
            model.TaxNumber = customer.IsPerson ? "T.C. " + customer.TCNo : customer.TaxNumber;
            model.ProductList = sepet.ProductList;
            model.TotalSalePrice = sepet.TotalSalePrice;
            model.PriceString = saleVM.PriceString;

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