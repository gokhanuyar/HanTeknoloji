using HanTeknoloji.Business.Manager;
using HanTeknoloji.Data.Models.Orm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HanTeknoloji.Web.Areas.Admin.Models.Attributes
{
    public class RolControl : ActionFilterAttribute
    {
        public int rolNumber = 0;

        public RolControl(EnumRoles role)
        {
            rolNumber = Convert.ToInt32(role);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            GenericRepository<AdminUser> repo = new GenericRepository<AdminUser>();
            string email = HttpContext.Current.User.Identity.Name;
            AdminUser admin = repo.FirstOrDefault(x => x.Email == email);

            string[] roles = admin.Roles.Split(';');

            bool yetkiVarMi = false;

            for (int i = 0; i < roles.Count(); i++)
            {
                int rol = string.IsNullOrEmpty(roles[i]) ? 0 : Convert.ToInt32(roles[i]);

                if (rolNumber == rol)
                {
                    yetkiVarMi = true;
                }
            }

            if (!yetkiVarMi)
            {
                filterContext.HttpContext.Response.Redirect("/Admin/AdminAuthority/Index");
            }
        }
    }
}