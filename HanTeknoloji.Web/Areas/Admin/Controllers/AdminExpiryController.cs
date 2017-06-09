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
        public ActionResult Customer(string name)
        {
            int customerId = Convert.ToInt32(name.Split(' ')[0].Replace("#", ""));
            var customer = rpcustomer.Find(customerId);
            CustomerExpiryWrapVM model = new CustomerExpiryWrapVM();
            model.CustomerList = Customers();
            model.ExpiryResultList = rpcustomerexpiry
                .GetListWithQuery(x => x.CustomerID == customerId)
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

        private List<CustomerExpiryVM> Suppliers()
        {
            return rpsupplier.GetAll().Select(x => new CustomerExpiryVM
            {
                CustomerID = x.ID,
                Name = x.CompanyName,
                Phone = x.Phone
            }).ToList();
        }

        public JsonResult Pay(CustomerExpiryVM model)
        {
            var customer = rpcustomer.Find(model.ID);
            customer.ExpiryValue -= model.Price;
            customer.PaidExpiryValue += model.Price;
            rpcustomer.SaveChanges();
            string postValue = "#" + customer.ID + " " + customer.Name + " " + customer.Phone;
            return Json(postValue, JsonRequestBehavior.AllowGet);
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

        public ActionResult Supplier()
        {
            return View();
        }
    }
}