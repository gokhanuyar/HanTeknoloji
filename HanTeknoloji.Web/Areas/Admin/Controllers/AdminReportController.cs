﻿using HanTeknoloji.Web.Areas.Admin.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace HanTeknoloji.Web.Areas.Admin.Controllers
{
    public class AdminReportController : AdminBaseController
    {
        public ActionResult ProductSale(int? page, string date)
        {
            int _page = page ?? 1;
            DateTime _date;
            if (!string.IsNullOrEmpty(date))
            {
                Session["date"] = date;
                _date = Convert.ToDateTime(date);
            }
            else
            {
                _date = Session["date"] == null ? DateTime.Now : Convert.ToDateTime(Session["date"]);
            }
            //DateTime _date = string.IsNullOrEmpty(beforeDate) ? DateTime.Now : Convert.ToDateTime(beforeDate);
            date = Session["date"] == null ? "" : Session["date"].ToString();
            if (date.Length == 7)
            {
                var list = rpsale
                                .GetListWithQuery(x => x
                                .AddDate.Month == _date.Month && x
                                .AddDate.Year == _date.Year && x
                                .CustomerID == 0)
                                .OrderByDescending(x => x.AddDate)
                                .ToList().Select(x => new ReportVM()
                                {
                                    PaymentType = x.PaymentType,
                                    SaleDate = String.Format("{0:d/M/yyyy}", x.AddDate),
                                    SaleTime = String.Format("{0:HH:mm}", x.AddDate),
                                    Price = x.Price,
                                    ProductID = x.ProductID,
                                    Quantity = x.Quantity,
                                    UserID = x.UserID,
                                    KdvPrice = x.KdvPrice
                                }).ToList();

                list.ForEach(l => l.Product = rpproduct.GetListWithQuery(x => x.ID == l.ProductID).Select(x => new ProductVM()
                {
                    TradeMark = rptrademark.Find(x.TradeMarkID).Name,
                    ProductModel = rpproductmodel.Find(x.ProductModelID).Name,
                    UnitPrice = x.UnitPrice
                }).FirstOrDefault());

                list.ForEach(l => l.AdminUserName = rpadminuser.Find(l.UserID).FullName);
                ViewBag.quantity = list.Sum(x => x.Quantity);
                ViewBag.unitprice = list.Sum(x => x.Product.UnitPrice * x.Quantity);
                ViewBag.saleprice = list.Sum(x => x.Price);
                ViewBag.kdv = list.Sum(x => x.KdvPrice);
                ViewBag.brut = ViewBag.saleprice - (ViewBag.unitprice + ViewBag.kdv);
                ViewBag.date = String.Format("{0:y}", _date);
                IPagedList<ReportVM> model = list.ToPagedList(_page, 3);
                return View(model);
            }
            else
            {
                var list = rpsale
                                .GetListWithQuery(x => x
                                .AddDate.Day == _date.Day && x
                                .AddDate.Month == _date.Month && x
                                .AddDate.Year == _date.Year && x
                                .CustomerID == 0)
                                .OrderByDescending(x => x.AddDate)
                                .ToList().Select(x => new ReportVM()
                                {
                                    PaymentType = x.PaymentType,
                                    SaleDate = String.Format("{0:d/M/yyyy}", x.AddDate),
                                    SaleTime = String.Format("{0:HH:mm}", x.AddDate),
                                    Price = x.Price,
                                    ProductID = x.ProductID,
                                    Quantity = x.Quantity,
                                    UserID = x.UserID,
                                    KdvPrice = x.KdvPrice
                                }).ToList();

                list.ForEach(l => l.Product = rpproduct.GetListWithQuery(x => x.ID == l.ProductID).Select(x => new ProductVM()
                {
                    TradeMark = rptrademark.Find(x.TradeMarkID).Name,
                    ProductModel = rpproductmodel.Find(x.ProductModelID).Name,
                    UnitPrice = x.UnitPrice
                }).FirstOrDefault());

                list.ForEach(l => l.AdminUserName = rpadminuser.Find(l.UserID).FullName);
                ViewBag.quantity = list.Sum(x => x.Quantity);
                ViewBag.unitprice = list.Sum(x => x.Product.UnitPrice * x.Quantity);
                ViewBag.saleprice = list.Sum(x => x.Price);
                ViewBag.kdv = list.Sum(x => x.KdvPrice);
                ViewBag.brut = ViewBag.saleprice - (ViewBag.unitprice + ViewBag.kdv);
                ViewBag.date = _date.ToLongDateString();
                IPagedList<ReportVM> model = list.ToPagedList(_page, 3);
                return View(model);
            }
        }

        public ActionResult InvoiceProductSale(int? page, string date)
        {
            int _page = page ?? 1;
            DateTime _date;
            if (!string.IsNullOrEmpty(date))
            {
                Session["date"] = date;
                _date = Convert.ToDateTime(date);
            }
            else
            {
                _date = Session["date"] == null ? DateTime.Now : Convert.ToDateTime(Session["date"]);
            }
            //DateTime _date = string.IsNullOrEmpty(beforeDate) ? DateTime.Now : Convert.ToDateTime(beforeDate);
            date = Session["date"] == null ? "" : Session["date"].ToString();
            if (date.Length == 7)
            {
                var list = rpsale
                                .GetListWithQuery(x => x
                                .AddDate.Month == _date.Month && x
                                .AddDate.Year == _date.Year && x
                                .CustomerID != 0)
                                .OrderByDescending(x => x.AddDate)
                                .ToList().Select(x => new ReportVM()
                                {
                                    CustomerID = x.CustomerID,
                                    PaymentType = x.PaymentType,
                                    SaleDate = String.Format("{0:d/M/yyyy}", x.AddDate),
                                    InvoiceDate = String.Format("{0:d/M/yyyy}", x.InvoiceDate),
                                    SaleTime = String.Format("{0:HH:mm}", x.AddDate),
                                    Price = x.Price,
                                    ProductID = x.ProductID,
                                    Quantity = x.Quantity,
                                    UserID = x.UserID,
                                    KdvPrice = x.KdvPrice
                                }).ToList();

                list.ForEach(l => l.Product = rpproduct.GetListWithQuery(x => x.ID == l.ProductID).Select(x => new ProductVM()
                {
                    TradeMark = rptrademark.Find(x.TradeMarkID).Name,
                    ProductModel = rpproductmodel.Find(x.ProductModelID).Name,
                    UnitPrice = x.UnitPrice
                }).FirstOrDefault());

                list.ForEach(l => l.AdminUserName = rpadminuser.Find(l.UserID).FullName);
                ViewBag.quantity = list.Sum(x => x.Quantity);
                ViewBag.unitprice = list.Sum(x => x.Product.UnitPrice * x.Quantity);
                ViewBag.saleprice = list.Sum(x => x.Price);
                ViewBag.kdv = list.Sum(x => x.KdvPrice);
                ViewBag.brut = ViewBag.saleprice - (ViewBag.unitprice + ViewBag.kdv);
                ViewBag.date = String.Format("{0:y}", _date);
                IPagedList<ReportVM> model = list.ToPagedList(_page, 3);
                return View(model);
            }
            else
            {
                var list = rpsale
                                .GetListWithQuery(x => x
                                .AddDate.Day == _date.Day && x
                                .AddDate.Month == _date.Month && x
                                .AddDate.Year == _date.Year && x
                                .CustomerID != 0)
                                .OrderByDescending(x => x.AddDate)
                                .ToList().Select(x => new ReportVM()
                                {
                                    CustomerID = x.CustomerID,
                                    PaymentType = x.PaymentType,
                                    SaleDate = String.Format("{0:d/M/yyyy}", x.AddDate),
                                    SaleTime = String.Format("{0:HH:mm}", x.AddDate),
                                    InvoiceDate = String.Format("{0:d/M/yyyy}", x.InvoiceDate),
                                    Price = x.Price,
                                    ProductID = x.ProductID,
                                    Quantity = x.Quantity,
                                    UserID = x.UserID,
                                    KdvPrice = x.KdvPrice
                                }).ToList();

                list.ForEach(l => l.Product = rpproduct.GetListWithQuery(x => x.ID == l.ProductID).Select(x => new ProductVM()
                {
                    TradeMark = rptrademark.Find(x.TradeMarkID).Name,
                    ProductModel = rpproductmodel.Find(x.ProductModelID).Name,
                    UnitPrice = x.UnitPrice
                }).FirstOrDefault());

                list.ForEach(l => l.AdminUserName = rpadminuser.Find(l.UserID).FullName);
                ViewBag.quantity = list.Sum(x => x.Quantity);
                ViewBag.unitprice = list.Sum(x => x.Product.UnitPrice * x.Quantity);
                ViewBag.saleprice = list.Sum(x => x.Price);
                ViewBag.kdv = list.Sum(x => x.KdvPrice);
                ViewBag.brut = ViewBag.saleprice - (ViewBag.unitprice + ViewBag.kdv);
                ViewBag.date = _date.ToLongDateString();
                IPagedList<ReportVM> model = list.ToPagedList(_page, 3);
                return View(model);
            }
        }

        public JsonResult GetCustomer(int id)
        {
            var customer = rpcustomer.Find(id);
            CustomerVM model = new CustomerVM()
            {
                Name = customer.Name,
                TCNo = customer.TCNo,
                Phone = customer.Phone,
                TaxOffice = customer.TaxOffice,
                TaxNumber = customer.TaxNumber,
                City = rpcity.Find(customer.CityID).Name,
                Region = rpregion.Find(customer.RegionID).Name,
                Address = customer.Address
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}