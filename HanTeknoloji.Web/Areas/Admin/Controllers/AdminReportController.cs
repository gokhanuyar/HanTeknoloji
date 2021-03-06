﻿using HanTeknoloji.Web.Areas.Admin.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using HanTeknoloji.Data.Models.Orm.Entity;
using HanTeknoloji.Web.Areas.Admin.Models.Attributes;
using HanTeknoloji.Web.Areas.Admin.Models.Dto;
using HanTeknoloji.Web.Areas.Admin.Models.Types.Enums;

namespace HanTeknoloji.Web.Areas.Admin.Controllers
{
    [RolControl(EnumRoles.Manager)]
    public class AdminReportController : AdminBaseController
    {
        public ActionResult ProductSale(int? page, FormCollection collection)
        {
            int _page = page ?? 1;
            FilterDto dto = new FilterDto { IsInvoiced = false };
            FilterOperations(collection, ref dto);
            var list = ReportOperations(dto, false);

            GetDropdownItems();
            return View(GetProductSaleReport(list, _page));
        }

        private IPagedList<ReportVM> GetProductSaleReport(List<Sale> sales, int _page)
        {
            var list = sales.Select(x => new ReportVM()
            {
                ID = x.ID,
                UserID = x.UserID
            }).ToList();

            list.ForEach(l => l.Details = rpsaledetails
            .GetListWithQuery(d => d.SaleID == l.ID)
            .Select(d => new SaleDetailsVM
            {
                ID = d.ID,
                AddDate = d.AddDate,
                KdvPrice = d.KdvPrice,
                Price = d.Price,
                ProductID = d.ProductID,
                Quantity = d.Quantity,
                UnitSalePrice = d.UnitSalePrice
            }).ToList());

            list.ForEach(l => l.Details.ForEach(d => d.Product = rpproduct.GetListWithQuery(p => p.ID == d.ProductID).Select(p => new ProductVM
            {
                TradeMark = rptrademark.Find(p.TradeMarkID).Name,
                ProductModel = rpproductmodel.Find(p.ProductModelID).Name
            }).FirstOrDefault()));

            list.ForEach(l => l.Details.ForEach(d => d.InfoList = rpsaledetailinfo.GetListWithQuery(i => i.SaleDetailID == d.ID).Select(i => new SaleDetailsInfoDto
            {
                Quantity = i.Quantity,
                UnitBuyPrice = rppaymentinfo.Find(i.PaymentInfoID).UnitPrice
            }).ToList()));

            //list.ForEach(l => l.AdminUserName = rpadminuser.Find(l.UserID).FullName);
            List<ReportVM> model = new List<ReportVM>();
            foreach (var sale in list)
            {
                foreach (var detail in sale.Details)
                {
                    ReportVM vm = new ReportVM()
                    {
                        ID = sale.ID,
                        Product = detail.Product,
                        Quantity = detail.Quantity,
                        KdvPrice = detail.KdvPrice,
                        Price = detail.Price + detail.KdvPrice,
                        UnitBuyPrice = detail.InfoList.Sum(x => x.UnitBuyPrice * x.Quantity) / detail.InfoList.Sum(x => x.Quantity),
                        UnitSalePrice = detail.UnitSalePrice
                    };
                    model.Add(vm);
                }
            }

            ViewBag.quantity = model.Sum(x => x.Quantity);
            ViewBag.unitprice = model.Sum(x => x.UnitBuyPrice * x.Quantity);
            ViewBag.saleprice = model.Sum(x => x.Price);
            ViewBag.kdv = model.Sum(x => x.KdvPrice);
            ViewBag.brut = ViewBag.saleprice - (ViewBag.unitprice + ViewBag.kdv);
            IPagedList<ReportVM> pagedModel = model.ToPagedList(_page, 20);
            return pagedModel;
        }

        public ActionResult InvoiceProductSale(int? page, FormCollection collection)
        {
            int _page = page ?? 1;
            FilterDto dto = new FilterDto { IsInvoiced = true };
            FilterOperations(collection, ref dto);
            var list = ReportOperations(dto, true);

            GetDropdownItems();
            return View(GetInvoiceProductSaleReport(list, _page));
        }

        private IPagedList<ReportVM> GetInvoiceProductSaleReport(List<Sale> sales, int _page)
        {
            var list = sales.Select(x => new ReportVM()
            {
                ID = x.ID,
                CustomerID = x.CustomerID
            }).ToList();

            list.ForEach(l => l.Details = rpsaledetails
            .GetListWithQuery(d => d.SaleID == l.ID)
            .Select(d => new SaleDetailsVM
            {
                ID = d.ID,
                AddDate = d.AddDate,
                KdvPrice = d.KdvPrice,
                Price = d.Price,
                ProductID = d.ProductID,
                Quantity = d.Quantity,
                UnitSalePrice = d.UnitSalePrice
            }).ToList());

            list.ForEach(l => l.Details.ForEach(d => d.Product = rpproduct.GetListWithQuery(p => p.ID == d.ProductID).Select(p => new ProductVM
            {
                TradeMark = rptrademark.Find(p.TradeMarkID).Name,
                ProductModel = rpproductmodel.Find(p.ProductModelID).Name
            }).FirstOrDefault()));

            list.ForEach(l => l.Details.ForEach(d => d.InfoList = rpsaledetailinfo.GetListWithQuery(i => i.SaleDetailID == d.ID).Select(i => new SaleDetailsInfoDto
            {
                Quantity = i.Quantity,
                UnitBuyPrice = rppaymentinfo.Find(i.PaymentInfoID).UnitPrice
            }).ToList()));

            //list.ForEach(l => l.AdminUserName = rpadminuser.Find(l.UserID).FullName);
            List<ReportVM> model = new List<ReportVM>();
            foreach (var sale in list)
            {
                foreach (var detail in sale.Details)
                {
                    ReportVM vm = new ReportVM()
                    {
                        ID = sale.ID,
                        Product = detail.Product,
                        Quantity = detail.Quantity,
                        KdvPrice = detail.KdvPrice,
                        Price = detail.Price + detail.KdvPrice,
                        CustomerID = sale.CustomerID,
                        UnitBuyPrice = detail.InfoList.Sum(x => x.UnitBuyPrice * x.Quantity) / detail.InfoList.Sum(x => x.Quantity),
                        UnitSalePrice = detail.UnitSalePrice
                    };
                    model.Add(vm);
                }
            }

            ViewBag.quantity = model.Sum(x => x.Quantity);
            ViewBag.unitprice = model.Sum(x => x.UnitBuyPrice * x.Quantity);
            ViewBag.saleprice = model.Sum(x => x.Price);
            ViewBag.kdv = model.Sum(x => x.KdvPrice);
            ViewBag.brut = ViewBag.saleprice - (ViewBag.unitprice + ViewBag.kdv);
            IPagedList<ReportVM> pagedModel = model.ToPagedList(_page, 20);
            return pagedModel;
        }

        public ActionResult Service(int? page, string date)
        {
            int _page = page ?? 1;
            ViewBag.page = _page - 1;
            DateTime _date;
            if (!string.IsNullOrEmpty(date))
            {
                Session["dateService"] = date;
                _date = Convert.ToDateTime(date);
            }
            else
            {
                _date = Session["dateService"] == null ? DateTime.Now : Convert.ToDateTime(Session["dateService"]);
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
                                    ID = x.ID,
                                    PaymentType = x.PaymentType,
                                    SaleTime = String.Format("{0:HH:mm}", x.AddDate),
                                    Price = x.Price,
                                    UserID = x.UserID,
                                    Note = x.Note
                                }).ToList();
                var costs = rptechservicecost.GetListWithQuery(x => x
                                  .AddDate.Month == _date.Month && x
                                  .AddDate.Year == _date.Year);

                ViewBag.saleprice = list.Sum(x => x.Price);
                ViewBag.quantity = list.Count;
                ViewBag.date = String.Format("{0:y}", _date);
                ViewBag.cost = costs.Sum(x => x.Cost);
                ViewBag.kar = ViewBag.saleprice - ViewBag.cost;
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
                                    ID = x.ID,
                                    PaymentType = x.PaymentType,
                                    SaleTime = String.Format("{0:HH:mm}", x.AddDate),
                                    Price = x.Price,
                                    UserID = x.UserID,
                                    Note = x.Note
                                }).ToList();

                var cost = rptechservicecost.FirstOrDefault(x => x
                                    .AddDate.Day == _date.Day && x
                                    .AddDate.Month == _date.Month && x
                                    .AddDate.Year == _date.Year);

                ViewBag.saleprice = list.Sum(x => x.Price);
                ViewBag.quantity = list.Count;
                ViewBag.date = _date.ToLongDateString();
                ViewBag.cost = cost != null ? cost.Cost : 0;
                ViewBag.kar = ViewBag.saleprice - ViewBag.cost;
                IPagedList<ReportVM> model = list.ToPagedList(_page, 20);
                return View(model);
            }
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

        public ActionResult SupplierReport(int? id, int? page)
        {
            ViewBag.Statuid = id;
            int _page = page ?? 1;
            List<ReportVM> list = new List<ReportVM>();
            if (id != null)
            {
                int supplierId = (int)id;
                list = rppaymentinfo
                    .GetListWithQuery(x => x.SupplierID == supplierId)
                    .Select(x => new ReportVM
                    {
                        ID = x.ID,
                        ProductID = x.ProductID,
                        PaymentType = x.Payment,
                        Quantity = x.BuyingCount,
                        SaleQuantity = x.Count,
                        UnitBuyPrice = x.UnitPrice,
                        SaleDate = String.Format("{0:d/M/yyyy}", x.AddDate),
                        SaleTime = String.Format("{0:HH:mm}", x.AddDate),
                        Price = x.UnitPrice * x.BuyingCount
                    }).ToList();

                list.ForEach(l => l.Product = rpproduct.GetListWithQuery(p => p.ID == l.ProductID).Select(p => new ProductVM
                {
                    TradeMark = rptrademark.Find(p.TradeMarkID).Name,
                    ProductModel = rpproductmodel.Find(p.ProductModelID).Name
                }).FirstOrDefault());

                ViewBag.quantity = list.Sum(x => x.Quantity);
                ViewBag.saleprice = list.Sum(x => x.Price);
                ViewBag.saleQuantity = ViewBag.quantity - list.Sum(x => x.SaleQuantity);
                string name = rpsupplier.Find((int)id).CompanyName;
                ViewBag.supplierName = name;
            }
            IPagedList<ReportVM> pagedModel = list.ToPagedList(_page, 20);
            GetSuppliers();
            return View(pagedModel);
        }

        public JsonResult GetPaymentDetails(int id)
        {
            var payment = rppaymentinfo.Find(id);
            PaymentDetailDto dto = new PaymentDetailDto();
            switch (payment.Payment)
            {
                case "Kredi Kartı":
                    dto.Payment = payment.Payment;
                    dto.BankName = payment.BankName;
                    dto.BankCardName = payment.BankCartName;
                    dto.CardNumber = payment.CartNumber;
                    break;
                case "Havale":
                    dto.Payment = payment.Payment;
                    dto.BankName = payment.BankName;
                    break;
                case "Vadeli":
                    dto.Payment = payment.Payment;
                    dto.ExpiryDate = payment.ExpiryDate.Value.ToLongDateString();
                    break;
                case "Çek":
                    dto.Payment = payment.Payment;
                    dto.BankName = payment.BankName;
                    dto.ExpiryDate = payment.ExpiryDate.Value.ToLongDateString();
                    dto.CheckNumber = payment.CheckNumber;
                    break;
            }
            return Json(dto, JsonRequestBehavior.AllowGet);
        }

        private void GetCustomers()
        {
            ViewData["customer"] = rpcustomer.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name + " Tel:" + x.Phone,
                Value = x.ID.ToString()
            }).ToList();
        }

        private void GetSuppliers()
        {
            ViewData["supplier"] = rpsupplier.GetAll().Select(x => new SelectListItem
            {
                Text = x.CompanyName + " Tel:" + x.Phone,
                Value = x.ID.ToString()
            }).ToList();
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

        public JsonResult GetSaleDetail(int id)
        {
            var sale = rpsale.Find(id);
            List<SaleDetailInfo> infoList = new List<SaleDetailInfo>();
            var detailList = rpsaledetails.GetListWithQuery(x => x.SaleID == sale.ID);
            foreach (var item in detailList)
            {
                var itemInfoList = rpsaledetailinfo.GetListWithQuery(x => x.SaleDetailID == item.ID);
                infoList.AddRange(itemInfoList);
            }

            List<string> imeiList = new List<string>();

            foreach (var item in infoList)
            {
                imeiList.Add(item.IMEI);
            }

            var vm = new ReportVM
            {
                PaymentType = sale.PaymentType,
                SaleDate = sale.AddDate.ToLongDateString(),
                SaleTime = String.Format("{0:HH:mm}", sale.AddDate),
                AdminUserName = rpadminuser.Find(sale.UserID).FullName,
                InvoiceDate = sale.InvoiceDate.Value.ToLongDateString(),
                ImeiList = imeiList
            };
            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServiceSaleDetail(int id)
        {
            var serviceSale = rpservicesale.Find(id);
            int? employeeId = (int?)serviceSale.EmployeeID;
            var employee = employeeId != 0 ? rpadminuser.Find(serviceSale.EmployeeID).FullName : "Belirtilmemiş";
            return Json(new
            {
                CustomerName = string.IsNullOrEmpty(serviceSale.CustomerName) ? "Belirtilmemiş" : serviceSale.CustomerName,
                ProductModel = string.IsNullOrEmpty(serviceSale.ProductModel) ? "Belirtilmemiş" : serviceSale.ProductModel,
                Employee = employee,
                IMEINumber = string.IsNullOrEmpty(serviceSale.IMEINumber) ? "Belirtilmemiş" : serviceSale.IMEINumber,
                SaleDate = String.Format("{0:d/M/yyyy}", serviceSale.AddDate),
                User = rpadminuser.Find(serviceSale.UserID).FullName
            }, JsonRequestBehavior.AllowGet);
        }

        private void GetDropdownItems()
        {
            ViewData["category"] = rpcategory.GetAll().Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = x.ID.ToString()
            });
            var proList = rpproduct.GetAll().Select(x => new ProductVM
            {
                ID = x.ID,
                TradeMark = rptrademark.Find(x.TradeMarkID).Name,
                ProductModel = rpproductmodel.Find(x.ProductModelID).Name
            }).ToList();
            ViewData["product"] = proList.Select(x => new SelectListItem
            {
                Text = x.TradeMark + " " + x.ProductModel,
                Value = x.ID.ToString()
            });
            ViewData["supplier"] = rpsupplier.GetAll().Select(x => new SelectListItem
            {
                Text = x.CompanyName,
                Value = x.ID.ToString()
            });
        }

        private FilterDto FilterOperations(FormCollection collection, ref FilterDto dto)
        {
            if (Convert.ToInt32(collection["_new"]) != 1)
            {
                var _form = dto.IsInvoiced ? (FormCollection)Session["invCollection"] : (FormCollection)Session["proCollection"];
                if (_form != null)
                {
                    dto.Date = _form["_date"];
                    dto.Month = _form["_month"];
                    dto.CategoryID = _form["_category"];
                    dto.ProductID = _form["_product"];
                    dto.Payment = _form["_payment"];
                    dto.DateSelect = _form["_dateSelect"];
                    //dto.SupplierID = _form["_supplier"];
                }
            }
            else
            {
                dto.Date = collection["_date"];
                dto.Month = collection["_month"];
                dto.CategoryID = collection["_category"];
                dto.ProductID = collection["_product"];
                dto.Payment = collection["_payment"];
                dto.DateSelect = collection["_dateSelect"];
                //dto.SupplierID = collection["_supplier"];
                //sayfalamada en son atılan sorguyu kaybetmemek için Session da tutuyoruz
                if (dto.IsInvoiced)
                {
                    Session["invCollection"] = collection;
                }
                else
                {
                    Session["proCollection"] = collection;
                }
            }
            return dto;
        }

        private List<Sale> ReportOperations(FilterDto dto, bool isInvoiced)
        {
            var list = rpsale.GetListWithQuery(x => x.IsInvoiced == isInvoiced).ToList();
            string selection = "";
            if (dto.DateSelect == "1")
            {
                var date = Convert.ToDateTime(dto.Date);
                list = list.Where(x => x
                                  .AddDate.Day == date.Day && x
                                  .AddDate.Month == date.Month && x
                                  .AddDate.Year == date.Year)
                                .OrderByDescending(x => x.AddDate)
                                .ToList();
                selection += date.ToLongDateString();
            }

            if (dto.DateSelect == "2")
            {
                var date = Convert.ToDateTime(dto.Date);
                list = list.Where(x => x
                                  .AddDate.Month == date.Month && x
                                  .AddDate.Year == date.Year)
                                .OrderByDescending(x => x.AddDate)
                                .ToList();
                selection += selection != "" ? " /// " + String.Format("{0:y}", date) : String.Format("{0:y}", date);
            }

            if (dto.Payment != "0" && dto.Payment != null)
            {
                list = list.Where(x => x.PaymentType == dto.Payment).ToList();
                selection += selection != "" ? " /// " + dto.Payment : dto.Payment;
            }

            if (!string.IsNullOrEmpty(dto.CategoryID))
            {
                int catId = Convert.ToInt32(dto.CategoryID);
                var addList = new List<Sale>();
                foreach (var item in list)
                {
                    foreach (var detail in item.SaleDetails)
                    {
                        if (detail.CategoryID == catId)
                        {
                            addList.Add(detail.Sale);
                            break;
                        }
                    }
                }
                list = addList;
                selection += selection != "" ? " /// " + rpcategory.Find(catId).CategoryName : rpcategory.Find(catId).CategoryName;
            }

            if (!string.IsNullOrEmpty(dto.ProductID))
            {
                int proId = Convert.ToInt32(dto.ProductID);
                var addList = new List<Sale>();
                foreach (var item in list)
                {
                    foreach (var detail in item.SaleDetails)
                    {
                        if (detail.ProductID == proId)
                        {
                            addList.Add(detail.Sale);
                            break;
                        }
                    }
                }
                list = addList;
                var pro = rpproduct.Find(proId);
                string name = rptrademark.Find(pro.TradeMarkID).Name + " " + rpproductmodel.Find(pro.ProductModelID).Name;
                selection += selection != "" ? " /// " + name : name;
            }

            //if (!string.IsNullOrEmpty(dto.SupplierID))
            //{
            //    int supplierId = Convert.ToInt32(dto.SupplierID);
            //    var supplier = rpsupplier.Find(supplierId);
            //    var dbPaymentInfoList = rppaymentinfo.GetListWithQuery(x => x.SupplierID == supplierId);

            //    var detailInfoList = new List<SaleDetailInfo>();
            //    foreach (var item in dbPaymentInfoList)
            //    {
            //        var infoList = rpsaledetailinfo.GetListWithQuery(x => x.PaymentInfoID == item.ID);
            //        detailInfoList.AddRange(infoList);
            //    }

            //    var detailIdList = new List<int>();
            //    foreach (var info in detailInfoList)
            //    {
            //        foreach (var sale in list)
            //        {
            //            var saleDetails = sale.SaleDetails.Where(x => x.ID == info.SaleDetailID).ToList();
            //            foreach (var item in saleDetails)
            //            {
            //                if (!detailIdList.Contains(item.ID))
            //                {
            //                    detailIdList.Add(item.ID);
            //                }
            //            }
            //        }
            //    }

            //    var saleList = new List<Sale>();

            //    foreach (var sale in list)
            //    {
            //        var detailList = new List<SaleDetails>();
            //        foreach (var detail in sale.SaleDetails)
            //        {                        
            //            foreach (var id in detailIdList)
            //            {
            //                if (detail.ID == id)
            //                {
            //                    saleList.Add(sale);
            //                }
            //            }
            //        }
            //    }

            //    list = saleList;
            //    selection += selection != "" ? " /// " + supplier.CompanyName : supplier.CompanyName;
            //}

            ViewBag.selection = selection == "" ? "Genel Liste" : selection;
            return list;
        }

        public ActionResult RemoveFilter()
        {
            string path = HttpContext.Request.UrlReferrer.PathAndQuery;
            Session.Remove("proCollection");
            Session.Remove("invCollection");
            return Redirect(path);
        }

        public ActionResult EditServiceSale(int id)
        {
            string path = HttpContext.Request.UrlReferrer.PathAndQuery;
            var entity = rpservicesale.Find(id);
            var model = new ServiceSaleVM
            {
                ID = entity.ID,
                CustomerName = entity.CustomerName,
                EmployeeID = entity.EmployeeID,
                IMEINumber = entity.IMEINumber,
                Note = entity.Note,
                PaymentType = entity.PaymentType,
                Price = entity.Price,
                ProductModel = entity.ProductModel,
                RedirectPath = path
            };
            GetAdminUsers();
            return View(model);
        }

        [HttpPost]
        public ActionResult EditServiceSale(ServiceSaleVM model)
        {
            if (ModelState.IsValid)
            {
                var entity = rpservicesale.Find(model.ID);
                entity.CustomerName = model.CustomerName;
                entity.EmployeeID = model.EmployeeID;
                entity.IMEINumber = model.IMEINumber;
                entity.Note = model.Note;
                entity.PaymentType = model.PaymentType;
                entity.Price = model.Price;
                entity.ProductModel = model.ProductModel;
                ViewBag.IslemDurum = EnumIslemDurum.Basarili;
                rpservicesale.SaveChanges();
            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
            }

            GetAdminUsers();
            return View(model);
        }

        private void GetAdminUsers()
        {
            ViewData["employee"] = rpadminuser.GetAll().Select(x => new SelectListItem
            {
                Text = x.FullName,
                Value = x.ID.ToString()
            }).ToList();

            ViewData["payment"] = new List<SelectListItem>
            {
                new SelectListItem{Value="Nakit",Text="Nakit"},
                new SelectListItem{Value="Kredi Kartı",Text="Kredi Kartı"},
            };
        }
    }
}