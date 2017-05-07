using HanTeknoloji.Data.Models.Orm.Entity;
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
    public class AdminSupplierController : AdminBaseController
    {
        public ActionResult Index(int? page, string searchString)
        {
            int _page = page ?? 1;

            List<SupplierVM> model;
            if (string.IsNullOrEmpty(searchString))
            {
                model = rpsupplier.GetAll().OrderByDescending(x => x.AddDate).Select(x => new SupplierVM
                {
                    ID = x.ID,
                    CompanyName = x.CompanyName,
                    Phone = x.Phone,
                    City = x.City,
                    Region = x.Region
                }).ToList();
            }
            else
            {
                string search = searchString.ToLower();
                model = rpsupplier.GetListWithQuery(x => x.CompanyName.ToLower().Contains(search)).OrderByDescending(x => x.AddDate).Select(x => new SupplierVM
                {
                    ID = x.ID,
                    CompanyName = x.CompanyName,
                    Phone = x.Phone,
                    City = x.City,
                    Region = x.Region
                }).ToList();
            }
            IPagedList<SupplierVM> list = model.ToPagedList(_page, 15);
            return View(list);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(SupplierVM model)
        {
            if (ModelState.IsValid)
            {
                Supplier entity = new Supplier
                {
                    CompanyName = model.CompanyName,
                    Address = model.Address,
                    Phone = model.Phone,
                    Fax = model.Fax,
                    WebSite = model.WebSite,
                    City = model.City,
                    Region = model.Region
                };
                rpsupplier.Add(entity);
                ViewBag.IslemDurum = EnumIslemDurum.Basarili;

            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
            }
            ModelState.Clear();
            return View();
        }

        public ActionResult Edit(int id)
        {
            var entity = rpsupplier.Find(id);
            SupplierVM model = new SupplierVM
            {
                ID = entity.ID,
                CompanyName = entity.CompanyName,
                Address = entity.Address,
                Phone = entity.Phone,
                Fax = entity.Fax,
                WebSite = entity.WebSite,
                City = entity.City,
                Region = entity.Region
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(SupplierVM model)
        {
            if (ModelState.IsValid)
            {
                Supplier entity = rpsupplier.Find(model.ID);
                entity.CompanyName = model.CompanyName;
                entity.Address = model.Address;
                entity.Phone = model.Phone;
                entity.Fax = model.Fax;
                entity.WebSite = model.WebSite;
                entity.City = model.City;
                entity.Region = model.Region;

                entity.UpdateDate = DateTime.Now;
                rpsupplier.SaveChanges();
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
            rpsupplier.Delete(id);
            return RedirectToAction("Index");
        }
    }
}