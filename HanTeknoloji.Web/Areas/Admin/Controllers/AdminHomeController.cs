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
            var saleDetails = rpsaledetails
                .GetListWithQuery(x => x.AddDate.Day == date.Day && x.AddDate.Month == date.Month && x.AddDate.Year == date.Year);

            var serviceSales=rpservicesale
                .GetListWithQuery(x => x.AddDate.Day == date.Day && x.AddDate.Month == date.Month && x.AddDate.Year == date.Year);

            var fewProducts = rpproduct.GetListWithQuery(x => x.Count < 5);

            HomeVM model = new HomeVM();
            model.SaleCount = saleDetails.Sum(x => x.Quantity);
            model.SaleTotal = saleDetails.Sum(x => x.Price + x.KdvPrice);

            model.ServiceSaleCount = serviceSales.Count;
            model.ServiceSaleTotal = serviceSales.Sum(x => x.Price);

            model.FewProductCount = fewProducts.Count;
            return View(model);
        }
    }
}