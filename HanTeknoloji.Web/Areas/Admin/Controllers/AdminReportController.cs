using HanTeknoloji.Web.Areas.Admin.Models.VM;
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
        public ActionResult Product(int? page, string date)
        {
            int _page = page ?? 1;
            DateTime _date = string.IsNullOrEmpty(date) ? DateTime.Now : Convert.ToDateTime(date);
            date = string.IsNullOrEmpty(date) ? "" : date;
            if (date.Length == 7)
            {
                var list = rpsale
                                .GetListWithQuery(x => x
                                .AddDate.Month == _date.Month && x
                                .AddDate.Year == _date.Year && x
                                .CustomerID == 0).ToList().Select(x => new ReportVM()
                                {
                                    CustomerID = x.CustomerID,
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
                IPagedList<ReportVM> model = list.ToPagedList(_page, 15);
                return View(model);
            }
            else
            {
                var list = rpsale
                                .GetListWithQuery(x => x
                                .AddDate.Day == _date.Day && x
                                .AddDate.Month == _date.Month && x
                                .AddDate.Year == _date.Year && x
                                .CustomerID == 0).ToList().Select(x => new ReportVM()
                                {
                                    CustomerID = x.CustomerID,
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
                IPagedList<ReportVM> model = list.ToPagedList(_page, 15);
                return View(model);
            }
        }
    }
}