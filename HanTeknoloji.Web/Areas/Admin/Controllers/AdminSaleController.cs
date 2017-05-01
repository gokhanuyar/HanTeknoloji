﻿using HanTeknoloji.Data.Models.Orm.Entity;
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
            return View(model);
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
                                UnitPrice = product.UnitPrice,
                                Count = product.Count,
                                SaleCount = 1,
                                TotalPrice = product.UnitPrice
                            };
                            model.ProductList.Add(pro);
                            model.TotalSalePrice += pro.TotalPrice;
                        }
                        Session["Sepet"] = model;
                    }
                    else
                    {
                        model = (CartVM)Session["Sepet"];
                        ViewBag.IslemDurum = EnumIslemDurum.StokYetersiz;
                        return View(model);
                    }
                }
                else
                {
                    model = (CartVM)Session["Sepet"];
                    ViewBag.IslemDurum = EnumIslemDurum.UrunYok;
                    return View(model);
                }
            }

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
            foreach (var item in sepet.ProductList)
            {
                Sale entity = new Sale()
                {
                    ProductID = item.ID,
                    PaymentType = model.PaymentType,
                    Price = item.TotalPrice,
                    Quantity = item.SaleCount
                };
                rpsale.Add(entity);
            }
            Session.Remove("yourSessionName");
            return RedirectToAction("Index");
        }
    }
}