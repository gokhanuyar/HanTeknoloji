using HanTeknoloji.Data.Models.Orm.Entity;
using HanTeknoloji.Web.Areas.Admin.Models.Types.Enums;
using HanTeknoloji.Web.Areas.Admin.Models.VM;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace HanTeknoloji.Web.Areas.Admin.Controllers
{
    public class AdminLoginController : AdminBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginVM model)
        {
            AdminUser user = rpadminuser.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);
            if (user != null)
            {
                model.ID = user.ID;
                user.LastLoginDate = DateTime.Now;
                rpadminuser.SaveChanges();
                IdentitySignin(model);

                return RedirectToAction("Index", "AdminHome");
            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ParolaYanlis;
                return View();
            }
        }

        public ActionResult SignOut()
        {
            IdentitySignout();
            return Redirect("/Admin/AdminLogin");
        }

        public void IdentitySignin(LoginVM model, string providerKey = null, bool isPersistent = false)
        {
            var claims = new List<Claim>();

            // create required claims
            claims.Add(new Claim(ClaimTypes.NameIdentifier, model.ID.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, model.Email));

            // custom – my serialized AppUserState object
            //claims.Add(new Claim("userState", "deneme"));

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignIn(new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            }, identity);
        }

        public void IdentitySignout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie,
                                            DefaultAuthenticationTypes.ExternalCookie);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
    }
}