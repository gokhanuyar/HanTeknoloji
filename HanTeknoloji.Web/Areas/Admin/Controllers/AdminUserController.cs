using HanTeknoloji.Web.Areas.Admin.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using HanTeknoloji.Data.Models.Orm.Entity;
using HanTeknoloji.Web.Areas.Admin.Models.Types.Enums;

namespace HanTeknoloji.Web.Areas.Admin.Controllers
{
    [Authorize]
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
            return View();
        }

        [HttpPost]
        public ActionResult Add(AdminUserVM model)
        {
            if (ModelState.IsValid)
            {
                AdminUser entity = new AdminUser
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password
                };
                rpadminuser.Add(entity);
                ViewBag.IslemDurum = EnumIslemDurum.Basarili;
                ModelState.Clear();
            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
            }            
            return View();
        }

        public ActionResult Edit(int id)
        {
            var entity = rpadminuser.Find(id);
            AdminUserVM model = new AdminUserVM
            {
                FullName = entity.FullName,
                Email = entity.Email,
                Password = entity.Password
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(AdminUserVM model)
        {
            if (ModelState.IsValid)
            {
                AdminUser entity = rpadminuser.Find(model.ID);
                entity.FullName = model.FullName;
                entity.Email = model.Email;
                entity.Password = model.Password;
                entity.UpdateDate = DateTime.Now;
                rpadminuser.SaveChanges();
                ViewBag.IslemDurum = EnumIslemDurum.Basarili;
            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            rpadminuser.Delete(id);
            return RedirectToAction("Index");
        }
    }
}