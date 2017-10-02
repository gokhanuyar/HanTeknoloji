using HanTeknoloji.Business.Manager;
using HanTeknoloji.Data.Models.Orm.Entity;
using HanTeknoloji.Web.Areas.Admin.Models.Types.Enums;
using HanTeknoloji.Web.Areas.Admin.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HanTeknoloji.Web.Areas.Admin.Controllers
{
    public class AdminLoginController : Controller
    {
        GenericRepository<AdminUser> rpadminuser = new GenericRepository<AdminUser>();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginVM model)
        {
            AdminUser user = rpadminuser.FirstOrDefault(x => (x.Email == model.Email || x.UserName == model.Email) && x.Password == model.Password);
            if (user != null)
            {
                user.LastLoginDate = DateTime.Now;
                rpadminuser.SaveChanges();
                FormsAuthentication.SetAuthCookie(user.Email, true);
                string controller = user.Roles.Split(';').Contains("1") ? "AdminHome" : "AdminSale";
                return RedirectToAction("Index", controller);
            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ParolaYanlis;
                return View();
            }
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}