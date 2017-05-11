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
                                    item.TotalPrice += item.UnitPrice;
                                    item.Count = product.Count;
                                    varMi = true;
                                    model.TotalSalePrice += item.UnitPrice;
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
                                UnitPrice = product.UnitSalePrice,
                                Count = product.Count,
                                SaleCount = 1,
                                TotalPrice = product.UnitSalePrice
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

            selectedItem.SaleCount = quantity;
            selectedItem.TotalPrice = selectedItem.UnitPrice * quantity;
            selectedItem.Count = product.Count;
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
                sepet.TotalSalePrice += selectedItem.TotalPrice;
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
                foreach (var item in sepet.ProductList)
                {
                    Sale entity = new Sale()
                    {
                        ProductID = item.ID,
                        PaymentType = model.PaymentType,
                        Price = item.TotalPrice,
                        Quantity = item.SaleCount,
                        UserID = userId,
                        CustomerID = model.CustomerID
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
            model.Address = customer.Address;
            model.City = rpcity.Find(customer.CityID).Name;
            model.Region = rpregion.Find(customer.RegionID).Name;
            model.TaxOffice = customer.TaxOffice;
            model.TaxNumber = customer.TaxNumber;
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
                    UserID = userId
                };
                rpservicesale.Add(entity);
            }
            ViewBag.IslemDurum = EnumIslemDurum.Basarili;
            return RedirectToAction("Index");
        }


    }
}