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
            context.MapRoute(
                name: "imei-count",
                url: "Admin/{controller}/{action}/{productId}/{supplierId}",
                defaults: new { controller = "AdminProduct", action = "GetImeiCount" }
            );

            context.MapRoute(
                name: "quantity",
                url: "Admin/{controller}/{action}/{id}/{quantity}",
                defaults: new { controller = "AdminSale", action = "ChangeQuantity", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "AdminSale", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}