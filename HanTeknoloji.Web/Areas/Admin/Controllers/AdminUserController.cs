using HanTeknoloji.Web.Areas.Admin.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using HanTeknoloji.Data.Models.Orm.Entity;
using HanTeknoloji.Web.Areas.Admin.Models.Types.Enums;
using HanTeknoloji.Web.Areas.Admin.Models.Attributes;

namespace HanTeknoloji.Web.Areas.Admin.Controllers
{
    [RolControl(EnumRoles.Manager)]
    public class AdminUserController : AdminBaseController
    {
        public ActionResult Index(int? page)
        {
            int _page = page ?? 1;
            var list = rpadminuser.GetAll().Select(x => new AdminUserVM()
            {
                ID = x.ID,
                FullName = x.FullName,
                Email = x.Email
            }).ToPagedList(_page, 10);
            return View(list);
        }

        public ActionResult Add()
        {
            GetRoles();
            return View();
        }

        private void GetRoles()
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem{Text="Süper Admin",Value="1"},
                new SelectListItem{Text="Satış Elemanı",Value="2"}
            };
            ViewData["roles"] = list;
        }

        [HttpPost]
        public ActionResult Add(AdminUserVM model)
        {
            if (rpadminuser.Any(x => x.Email == model.Email))
            {
                ViewBag.IslemDurum = EnumIslemDurum.AdminMevcut;
                GetRoles();
                return View(model);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    AdminUser entity = new AdminUser
                    {
                        FullName = model.FullName,
                        Email = model.Email,
                        Password = model.Password,
                        Roles = model.Role + ";"
                    };
                    rpadminuser.Add(entity);
                    ViewBag.IslemDurum = EnumIslemDurum.Basarili;
                    ModelState.Clear();
                }
                else
                {
                    ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
                }
                GetRoles();
                return View(model);
            }
        }

        public ActionResult Edit(int id)
        {
            var entity = rpadminuser.Find(id);
            AdminUserVM model = new AdminUserVM
            {
                FullName = entity.FullName,
                Email = entity.Email,
                Password = entity.Password,
                Role = entity.Roles.Replace(";", "")
            };
            GetRoles();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AdminUserVM model)
        {
            if (rpadminuser.Any(x => x.Email == model.Email && x.ID != model.ID))
            {
                ViewBag.IslemDurum = EnumIslemDurum.AdminMevcut;
                GetRoles();
                return View(model);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    AdminUser entity = rpadminuser.Find(model.ID);
                    string[] roles = entity.Roles.Split(';');
                    entity.FullName = model.FullName;
                    entity.Email = model.Email;
                    entity.Password = model.Password;
                    entity.UpdateDate = DateTime.Now;
                    if (!roles.Contains(model.Role))
                    {
                        entity.Roles = model.Role + ";";
                    }
                    rpadminuser.SaveChanges();
                    ViewBag.IslemDurum = EnumIslemDurum.Basarili;
                }
                else
                {
                    ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
                }
                GetRoles();
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            rpadminuser.Delete(id);
            return RedirectToAction("Index");
        }
    }
}