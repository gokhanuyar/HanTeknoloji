using System.Web.Mvc;

namespace HanTeknoloji.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("imei-count", "Admin/AdminProduct/GetImeiCount/{productId}/{supplierId}", new
            {
                controller = "AdminProduct",
                action = "GetImeiCount"
            });

            context.MapRoute("quantity", "Admin/AdminSale/ChangeQuantity/{id}/{quantity}", new
            {
                controller = "AdminSale",
                action = "ChangeQuantity",
                id = UrlParameter.Optional,
                quantity = UrlParameter.Optional
            });

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "AdminSale", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}