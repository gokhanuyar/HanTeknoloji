using HanTeknoloji.Web.Areas.Admin.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using HanTeknoloji.Data.Models.Orm.Entity;

namespace HanTeknoloji.Web.Areas.Admin.Controllers
{
    [Authorize]
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
                var sales = rpsale
                                .GetListWithQuery(x => x
                                .AddDate.Month == _date.Month && x
                                .AddDate.Year == _date.Year && x
                                .IsInvoiced == false)
                                .OrderByDescending(x => x.AddDate)
                                .ToList();
                ViewBag.date = String.Format("{0:y}", _date);
                return View(GetProductSaleReport(sales, _page));
            }
            else
            {
                var sales = rpsale
                                .GetListWithQuery(x => x
                                .AddDate.Day == _date.Day && x
                                .AddDate.Month == _date.Month && x
                                .AddDate.Year == _date.Year && x
                                .IsInvoiced == false)
                                .OrderByDescending(x => x.AddDate)
                                .ToList();
                ViewBag.date = _date.ToLongDateString();
                return View(GetProductSaleReport(sales, _page));
            }
        }

        private IPagedList<ReportVM> GetProductSaleReport(List<Sale> sales, int _page)
        {
            var list = sales.Select(x => new ReportVM()
            {
                ID = x.ID,
                PaymentType = x.PaymentType,
                SaleDate = String.Format("{0:d/M/yyyy}", x.AddDate),
                SaleTime = String.Format("{0:HH:mm}", x.AddDate),
                UserID = x.UserID
            }).ToList();

            list.ForEach(l => l.Details = rpsaledetails
            .GetListWithQuery(d => d.SaleID == l.ID)
            .Select(d => new SaleDetailsVM
            {
                AddDate = d.AddDate,
                KdvPrice = d.KdvPrice,
                Price = d.Price,
                ProductID = d.ProductID,
                Quantity = d.Quantity
            }).ToList());

            list.ForEach(l => l.Details.ForEach(d => d.Product = rpproduct.GetListWithQuery(p => p.ID == d.ProductID).Select(p => new ProductVM
            {
                TradeMark = rptrademark.Find(p.TradeMarkID).Name,
                ProductModel = rpproductmodel.Find(p.ProductModelID).Name,
                UnitPrice = p.UnitPrice,
                UnitSalePrice = p.UnitSalePrice
            }).FirstOrDefault()));

            list.ForEach(l => l.AdminUserName = rpadminuser.Find(l.UserID).FullName);
            List<ReportVM> model = new List<ReportVM>();
            foreach (var sale in list)
            {
                foreach (var detail in sale.Details)
                {
                    ReportVM vm = new ReportVM()
                    {
                        AdminUserName = sale.AdminUserName,
                        Product = detail.Product,
                        Quantity = detail.Quantity,
                        KdvPrice = detail.KdvPrice,
                        Price = detail.Price + detail.KdvPrice,
                        SaleDate = sale.SaleDate,
                        SaleTime = sale.SaleTime,
                        PaymentType = sale.PaymentType
                    };
                    model.Add(vm);
                }
            }

            ViewBag.quantity = model.Sum(x => x.Quantity);
            ViewBag.unitprice = model.Sum(x => x.Product.UnitPrice * x.Quantity);
            ViewBag.saleprice = model.Sum(x => x.Price);
            ViewBag.kdv = model.Sum(x => x.KdvPrice);
            ViewBag.brut = ViewBag.saleprice - (ViewBag.unitprice + ViewBag.kdv);            
            IPagedList<ReportVM> pagedModel = model.ToPagedList(_page, 20);
            return pagedModel;
        }

        public ActionResult InvoiceProductSale(int? page, string date)
        {
            int _page = page ?? 1;
            DateTime _date;
            if (!string.IsNullOrEmpty(date))
            {
                Session["invoicedDate"] = date;
                _date = Convert.ToDateTime(date);
            }
            else
            {
                _date = Session["invoicedDate"] == null ? DateTime.Now : Convert.ToDateTime(Session["invoicedDate"]);
            }
            date = Session["invoicedDate"] == null ? "" : Session["invoicedDate"].ToString();
            if (date.Length == 7)
            {
                var sales = rpsale
                                .GetListWithQuery(x => x
                                .AddDate.Month == _date.Month && x
                                .AddDate.Year == _date.Year && x
                                .IsInvoiced)
                                .OrderByDescending(x => x.AddDate)
                                .ToList();
                ViewBag.date = String.Format("{0:y}", _date);
                return View(GetInvoiceProductSaleReport(sales, _page));
            }
            else
            {
                var sales = rpsale
                                .GetListWithQuery(x => x
                                .AddDate.Day == _date.Day && x
                                .AddDate.Month == _date.Month && x
                                .AddDate.Year == _date.Year && x
                                .IsInvoiced)
                                .OrderByDescending(x => x.AddDate)
                                .ToList();
                ViewBag.date = _date.ToLongDateString();
                return View(GetInvoiceProductSaleReport(sales, _page));
            }
        }

        private IPagedList<ReportVM> GetInvoiceProductSaleReport(List<Sale> sales, int _page)
        {
            var list = sales.Select(x => new ReportVM()
            {
                ID = x.ID,
                PaymentType = x.PaymentType,
                SaleDate = String.Format("{0:d/M/yyyy HH:mm}", x.AddDate),
                InvoiceDate = String.Format("{0:d/M/yyyy HH:mm}", x.InvoiceDate),
                UserID = x.UserID,
                CustomerID = x.CustomerID
            }).ToList();

            list.ForEach(l => l.Details = rpsaledetails
            .GetListWithQuery(d => d.SaleID == l.ID)
            .Select(d => new SaleDetailsVM
            {
                AddDate = d.AddDate,
                KdvPrice = d.KdvPrice,
                Price = d.Price,
                ProductID = d.ProductID,
                Quantity = d.Quantity
            }).ToList());

            list.ForEach(l => l.Details.ForEach(d => d.Product = rpproduct.GetListWithQuery(p => p.ID == d.ProductID).Select(p => new ProductVM
            {
                TradeMark = rptrademark.Find(p.TradeMarkID).Name,
                ProductModel = rpproductmodel.Find(p.ProductModelID).Name,
                UnitPrice = p.UnitPrice,
                UnitSalePrice = p.UnitSalePrice
            }).FirstOrDefault()));

            list.ForEach(l => l.AdminUserName = rpadminuser.Find(l.UserID).FullName);
            List<ReportVM> model = new List<ReportVM>();
            foreach (var sale in list)
            {
                foreach (var detail in sale.Details)
                {
                    ReportVM vm = new ReportVM()
                    {
                        AdminUserName = sale.AdminUserName,
                        Product = detail.Product,
                        Quantity = detail.Quantity,
                        KdvPrice = detail.KdvPrice,
                        Price = detail.Price + detail.KdvPrice,
                        SaleDate = sale.SaleDate,
                        InvoiceDate = sale.InvoiceDate,
                        CustomerID = sale.CustomerID,
                        PaymentType = sale.PaymentType
                    };
                    model.Add(vm);
                }
            }

            ViewBag.quantity = model.Sum(x => x.Quantity);
            ViewBag.unitprice = model.Sum(x => x.Product.UnitPrice * x.Quantity);
            ViewBag.saleprice = model.Sum(x => x.Price);
            ViewBag.kdv = model.Sum(x => x.KdvPrice);
            ViewBag.brut = ViewBag.saleprice - (ViewBag.unitprice + ViewBag.kdv);
            IPagedList<ReportVM> pagedModel = model.ToPagedList(_page, 20);
            return pagedModel;
        }

        public ActionResult Service(int? page, string date)
        {
            int _page = page ?? 1;
            DateTime _date;
            if (!string.IsNullOrEmpty(date))
            {
                Session["dateService"] = date;
                _date = Convert.ToDateTime(date);
            }
            else
            {
                _date = Session["dateService"] == null ? DateTime.Now : Convert.ToDateTime(Session["date"]);
            }
            date = Session["dateService"] == null ? "" : Session["dateService"].ToString();

            if (date.Length == 7)
            {
                var list = rpservicesale.GetListWithQuery(x => x
                                  .AddDate.Month == _date.Month && x
                                  .AddDate.Year == _date.Year)
                                .OrderByDescending(x => x.AddDate)
                                .ToList().Select(x => new ReportVM()
                                {
                                    PaymentType = x.PaymentType,
                                    SaleDate = String.Format("{0:d/M/yyyy}", x.AddDate),
                                    SaleTime = String.Format("{0:HH:mm}", x.AddDate),
                                    Price = x.Price,
                                    UserID = x.UserID,
                                    Note = x.Note
                                }).ToList();
                list.ForEach(l => l.AdminUserName = rpadminuser.Find(l.UserID).FullName);
                ViewBag.saleprice = list.Sum(x => x.Price);
                ViewBag.quantity = list.Count;
                ViewBag.date = String.Format("{0:y}", _date);
                IPagedList<ReportVM> model = list.ToPagedList(_page, 20);
                return View(model);
            }
            else
            {
                var list = rpservicesale.GetListWithQuery(x => x
                                  .AddDate.Day == _date.Day && x
                                  .AddDate.Month == _date.Month && x
                                  .AddDate.Year == _date.Year)
                                .OrderByDescending(x => x.AddDate)
                                .ToList().Select(x => new ReportVM()
                                {
                                    PaymentType = x.PaymentType,
                                    SaleDate = String.Format("{0:d/M/yyyy}", x.AddDate),
                                    SaleTime = String.Format("{0:HH:mm}", x.AddDate),
                                    Price = x.Price,
                                    UserID = x.UserID,
                                    Note = x.Note
                                }).ToList();
                list.ForEach(l => l.AdminUserName = rpadminuser.Find(l.UserID).FullName);
                ViewBag.saleprice = list.Sum(x => x.Price);
                ViewBag.quantity = list.Count;
                ViewBag.date = _date.ToLongDateString();
                IPagedList<ReportVM> model = list.ToPagedList(_page, 20);
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
                Address = customer.Address,
                IsPerson = customer.IsPerson
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerReport(int? id, int? page)
        {
            int _page = page ?? 1;
            int _customerId;
            if (id != null)
            {
                Session["customerId"] = id;
                _customerId = Convert.ToInt32(id);
            }
            else
            {
                _customerId = Session["customerId"] == null ? 0 : Convert.ToInt32(Session["customerId"]);
            }
            List<ReportVM> model = new List<ReportVM>();
            if (_customerId != 0)
            {

                var sales = rpsale.GetListWithQuery(x => x.CustomerID == _customerId).ToList();

                var list = sales.Select(x => new ReportVM()
                {
                    ID = x.ID,
                    PaymentType = x.PaymentType,
                    SaleDate = String.Format("{0:d/M/yyyy}", x.AddDate),
                    SaleTime = String.Format("{0:HH:mm}", x.AddDate),
                    UserID = x.UserID
                }).ToList();

                list.ForEach(l => l.Details = rpsaledetails
                .GetListWithQuery(d => d.SaleID == l.ID)
                .Select(d => new SaleDetailsVM
                {
                    AddDate = d.AddDate,
                    KdvPrice = d.KdvPrice,
                    Price = d.Price,
                    ProductID = d.ProductID,
                    Quantity = d.Quantity
                }).ToList());

                list.ForEach(l => l.Details.ForEach(d => d.Product = rpproduct.GetListWithQuery(p => p.ID == d.ProductID).Select(p => new ProductVM
                {
                    TradeMark = rptrademark.Find(p.TradeMarkID).Name,
                    ProductModel = rpproductmodel.Find(p.ProductModelID).Name,
                    UnitPrice = p.UnitPrice,
                    UnitSalePrice = p.UnitSalePrice
                }).FirstOrDefault()));

                list.ForEach(l => l.AdminUserName = rpadminuser.Find(l.UserID).FullName);

                foreach (var sale in list)
                {
                    foreach (var detail in sale.Details)
                    {
                        ReportVM vm = new ReportVM()
                        {
                            AdminUserName = sale.AdminUserName,
                            Product = detail.Product,
                            Quantity = detail.Quantity,
                            KdvPrice = detail.KdvPrice,
                            Price = detail.Price + detail.KdvPrice,
                            SaleDate = sale.SaleDate,
                            SaleTime = sale.SaleTime,
                            PaymentType = sale.PaymentType
                        };
                        model.Add(vm);
                    }
                }

                ViewBag.quantity = model.Sum(x => x.Quantity);
                ViewBag.unitprice = model.Sum(x => x.Product.UnitPrice * x.Quantity);
                ViewBag.saleprice = model.Sum(x => x.Price);
                ViewBag.kdv = model.Sum(x => x.KdvPrice);
                ViewBag.brut = ViewBag.saleprice - (ViewBag.unitprice + ViewBag.kdv);
                string name = rpcustomer.Find(Convert.ToInt32(_customerId)).Name;
                ViewBag.customerName = name;
            }
            IPagedList<ReportVM> pagedModel = model.ToPagedList(_page, 20);
            GetCustomers();
            return View(pagedModel);
        }

        private void GetCustomers()
        {
            ViewData["customer"] = rpcustomer.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name + " Tel:" + x.Phone,
                Value = x.ID.ToString()
            }).ToList();
        }
    }
}