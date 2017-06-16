using HanTeknoloji.Data.Models.Orm.Entity;
using HanTeknoloji.Web.Areas.Admin.Models.Attributes;
using HanTeknoloji.Web.Areas.Admin.Models.Types.Enums;
using HanTeknoloji.Web.Areas.Admin.Models.VM;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HanTeknoloji.Web.Areas.Admin.Controllers
{    
    [RolControl(EnumRoles.Manager)]
    public class AdminCategoryController : AdminBaseController
    {
        public ActionResult Index(int? page, string searchString)
        {
            int _page = page ?? 1;

            List<CategoryVM> model;
            if (string.IsNullOrEmpty(searchString))
            {
                model = rpcategory.GetAll().OrderByDescending(x => x.AddDate).Select(x => new CategoryVM
                {
                    ID = x.ID,
                    CategoryName = x.CategoryName
                }).ToList();
            }
            else
            {
                string search = searchString.ToLower();
                model = rpcategory.GetListWithQuery(x => x.CategoryName.ToLower().Contains(search)).OrderByDescending(x => x.AddDate).Select(x => new CategoryVM
                {
                    ID = x.ID,
                    CategoryName = x.CategoryName
                }).ToList();
            }
            IPagedList<CategoryVM> list = model.ToPagedList(_page, 15);
            return View(list);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(CategoryVM model)
        {
            if (ModelState.IsValid)
            {
                Category entity = new Category
                {
                    CategoryName = model.CategoryName
                };
                rpcategory.Add(entity);
                ViewBag.IslemDurum = EnumIslemDurum.Basarili;

            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var entity = rpcategory.Find(id);
            CategoryVM model = new CategoryVM
            {
                ID = entity.ID,
                CategoryName = entity.CategoryName
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CategoryVM model)
        {
            if (ModelState.IsValid)
            {
                Category entity = rpcategory.Find(model.ID);
                entity.CategoryName = model.CategoryName;
                entity.UpdateDate = DateTime.Now;
                rpcategory.SaveChanges();
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
            rpcategory.Delete(id);
            return RedirectToAction("Index");
        }
    }
}