using HanTeknoloji.Web.Areas.Admin.Models.Attributes;
using HanTeknoloji.Web.Areas.Admin.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HanTeknoloji.Web.Areas.Admin.Controllers
{
    [RolControl(EnumRoles.Manager)]
    public class AdminHomeController : AdminBaseController
    {
        public ActionResult Index()
        {
            var date = DateTime.Now;
            var weekDate = DateTime.Now.AddDays(7);

            var saleDetails = rpsaledetails
                .GetListWithQuery(x => x.AddDate.Day == date.Day && x.AddDate.Month == date.Month && x.AddDate.Year == date.Year);

            var serviceSales = rpservicesale
                .GetListWithQuery(x => x.AddDate.Day == date.Day && x.AddDate.Month == date.Month && x.AddDate.Year == date.Year);

            var fewProducts = rpproduct.GetListWithQuery(x => x.Count < 5);

            var supplierExpiry = rpsupplierexpiry.GetAll();
            var customerExpiry = rpcustomerexpiry.GetAll();

            var supplierExpiryWeekly = supplierExpiry.Where(x => x.ExpiryDate.Day >= date.Day && x.ExpiryDate.Month >= date.Month && x.ExpiryDate.Year >= date.Year && x.ExpiryDate.Day <= weekDate.Day && x.ExpiryDate.Month <= weekDate.Month && x.ExpiryDate.Year <= weekDate.Year).ToList();

            var customerExpiryWeekly = customerExpiry.Where(x => x.ExpiryDate.Day >= date.Day && x.ExpiryDate.Month >= date.Month && x.ExpiryDate.Year >= date.Year && x.ExpiryDate.Day <= weekDate.Day && x.ExpiryDate.Month <= weekDate.Month && x.ExpiryDate.Year <= weekDate.Year).ToList();

            var supplierExpiryPast = supplierExpiry.Where(x => x.ExpiryDate.Day < date.Day && x.ExpiryDate.Month <= date.Month && x.ExpiryDate.Year <= date.Year).ToList();

            var customerExpiryPast = customerExpiry.Where(x => x.ExpiryDate.Day < date.Day && x.ExpiryDate.Month <= date.Month && x.ExpiryDate.Year <= date.Year).ToList();

            HomeVM model = new HomeVM();
            model.SaleCount = saleDetails.Sum(x => x.Quantity);
            model.SaleTotal = saleDetails.Sum(x => x.Price + x.KdvPrice);

            model.ServiceSaleCount = serviceSales.Count;
            model.ServiceSaleTotal = serviceSales.Sum(x => x.Price);

            model.FewProductCount = fewProducts.Count;

            model.SupplierExpiryCount = supplierExpiryWeekly.Count;
            model.SupplierExpiryValue = supplierExpiryWeekly.Sum(x => x.TotalBuyingPrice - x.PaidPrice);

            model.CustomerExpiryCount = customerExpiryWeekly.Count;
            model.CustomerExpiryValue = customerExpiryWeekly.Sum(x => x.SaleTotalPrice - x.PaidPrice);

            model.SupplierExpiryPastedCount = supplierExpiryPast.Count;
            model.SupplierExpiryPastedValue = supplierExpiryPast.Sum(x => x.TotalBuyingPrice - x.PaidPrice);

            model.CustomerExpiryPastedCount = customerExpiryPast.Count;
            model.CustomerExpiryPastedValue = customerExpiryPast.Sum(x => x.SaleTotalPrice - x.PaidPrice);
            return View(model);
        }
    }
}