using HanTeknoloji.Data.Models.Orm.Entity;
using HanTeknoloji.Web.Areas.Admin.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HanTeknoloji.Web.Areas.Admin.Controllers
{
    public class AdminExpiryController : AdminBaseController
    {
        public ActionResult Customer()
        {
            CustomerExpiryWrapVM model = new CustomerExpiryWrapVM();
            model.ExpiryResultList = new List<ExpiryResultVM>();
            model.CustomerList = Customers();
            return View(model);
        }

        [HttpPost]
        public ActionResult Customer(int id)
        {
            var customer = rpcustomer.Find(id);
            CustomerExpiryWrapVM model = new CustomerExpiryWrapVM();
            model.CustomerList = Customers();
            model.ExpiryResultList = new List<ExpiryResultVM>();
            if (customer != null)
            {
                model.ExpiryResultList = rpcustomerexpiry
                                .GetListWithQuery(x => x.CustomerID == id)
                                .OrderBy(x => x.ExpiryDate)
                                .Select(x => new ExpiryResultVM
                                {
                                    ID = x.ID,
                                    ExpiryDate = x.ExpiryDate.ToLongDateString(),
                                    SaleDate = x.AddDate.ToLongDateString(),
                                    SalePrice = x.SaleTotalPrice,
                                    PaidPrice = x.SaleTotalPrice - x.ExpiryValue,
                                    ExpiryValue = x.ExpiryValue
                                }).ToList();
                model.TotalExpiryValue = customer.ExpiryValue;
                model.CustomerName = customer.Name + "  İçin Vadeli Satış Tablosu";
                model.CustomerID = customer.ID;
                model.PaidExpiryValue = customer.PaidExpiryValue;
            }

            return View(model);
        }

        public ActionResult Supplier()
        {
            SupplierExpiryWrapVM model = new SupplierExpiryWrapVM();
            model.ExpiryResultList = new List<ExpiryResultVM>();
            model.SupplierList = Suppliers();
            return View(model);
        }

        [HttpPost]
        public ActionResult Supplier(int id)
        {
            var supplier = rpsupplier.Find(id);
            SupplierExpiryWrapVM model = new SupplierExpiryWrapVM();
            model.SupplierList = Suppliers();
            model.ExpiryResultList = new List<ExpiryResultVM>();
            if (supplier != null)
            {
                model.ExpiryResultList = rpsupplierexpiry
                                .GetListWithQuery(x => x.SupplierID == id)
                                .OrderBy(x => x.ExpiryDate)
                                .Select(x => new ExpiryResultVM
                                {
                                    ID = x.ID,
                                    ExpiryDate = x.ExpiryDate.ToLongDateString(),
                                    SaleDate = x.AddDate.ToLongDateString(),
                                    SalePrice = x.TotalBuyingPrice,
                                    PaidPrice = x.PaidPrice,
                                    ExpiryValue = x.TotalBuyingPrice - x.PaidPrice
                                }).ToList();
                model.TotalExpiryValue = supplier.TotalExpiryValue;
                model.SupplierName = supplier.CompanyName + "  İçin Vadeli Satış Tablosu";
                model.SupplierID = supplier.ID;
                model.PaidExpiryValue = supplier.PaidExpiryValue;
            }

            return View(model);
        }

        private List<CustomerExpiryVM> Customers()
        {
            return rpcustomer.GetAll().Select(x => new CustomerExpiryVM
            {
                CustomerID = x.ID,
                Name = x.Name,
                Phone = x.Phone
            }).ToList();
        }

        private List<SupplierExpiryVM> Suppliers()
        {
            return rpsupplier.GetAll().Select(x => new SupplierExpiryVM
            {
                SupplierID = x.ID,
                Name = x.CompanyName,
                Phone = x.Phone
            }).ToList();
        }

        private decimal Calculate(Product pro, int count)
        {
            return pro.UnitPrice * count;
        }

        public JsonResult GetSaleDetails(int id)
        {
            var expiry = rpcustomerexpiry.Find(id);
            var sale = rpsale.Find(expiry.SaleID);
            var list = sale.SaleDetails.Select(x => new ReportVM
            {
                Quantity = x.Quantity,
                Price = x.Price + x.KdvPrice,
                ProductID = x.ProductID
            }).ToList();

            list.ForEach(l => l.Product = rpproduct
            .GetListWithQuery(p => p.ID == l.ProductID)
            .Select(p => new ProductVM
            {
                TradeMark = rptrademark.Find(p.TradeMarkID).Name,
                ProductModel = rpproductmodel.Find(p.ProductModelID).Name,
            }).FirstOrDefault());

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductDetails(int id)
        {
            var expiry = rpsupplierexpiry.Find(id);
            var product = rpproduct.Find(expiry.ProductID);
            string trademark = rptrademark.Find(product.TradeMarkID).Name;
            string model = rpproductmodel.Find(product.ProductModelID).Name;
            var pro = new ReportVM()
            {
                Quantity = expiry.ProductCount,
                Price = Calculate(product, expiry.ProductCount),
                Note = trademark + " " + model
            };
            return Json(pro, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Pay(CustomerExpiryVM model)
        {
            var customer = rpcustomer.Find(model.ID);
            customer.ExpiryValue -= model.Price;
            customer.PaidExpiryValue += model.Price;
            rpcustomer.SaveChanges();
            var expiryPayment = new ExpiryPayment
            {
                Price = model.Price,
                PersonID = model.ID,
                AdminUserID = UserID()
            };
            rpexpirypayment.Add(expiryPayment);
            //string postValue = "#" + customer.ID + " " + customer.Name + " " + customer.Phone;
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult PaySupplier(SupplierExpiryVM model)
        {
            var supplier = rpsupplier.Find(model.ID);
            supplier.TotalExpiryValue -= model.Price;
            supplier.PaidExpiryValue += model.Price;
            rpsupplier.SaveChanges();
            var expiryPayment = new ExpiryPayment
            {
                Price = model.Price,
                PersonID = model.ID,
                AdminUserID = UserID()
            };
            rpexpirypayment.Add(expiryPayment);
            //string postValue = "#" + supplier.ID + " " + supplier.CompanyName + " " + supplier.Phone;
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPayments(int id)
        {
            var list = rpexpirypayment
                .GetListWithQuery(x => x.PersonID == id)
                .OrderByDescending(x => x.AddDate)
                .Select(x => new ExpiryPaymentVM
                {
                    Date = x.AddDate.ToLongDateString(),
                    Price = x.Price,
                    AdminUserName = rpadminuser.Find(x.AdminUserID).FullName,
                    Hour = String.Format("{0:HH:mm}", x.AddDate)
                }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}