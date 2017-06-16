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
                    Phone = x.Phone ?? "-",
                    Fax = x.Fax ?? "-",
                    WebSite = x.WebSite ?? "-",
                    City = rpcity.Find(x.CityID).Name,
                    Region = rpregion.Find(x.RegionID).Name,
                    Address = x.Address ?? "-"
                }).ToList();
            }
            else
            {
                string search = searchString.ToLower();
                model = rpsupplier.GetListWithQuery(x => x.CompanyName.ToLower().Contains(search)).OrderByDescending(x => x.AddDate).Select(x => new SupplierVM
                {
                    ID = x.ID,
                    CompanyName = x.CompanyName,
                    Phone = x.Phone ?? "-",
                    Fax = x.Fax ?? "-",
                    WebSite = x.WebSite ?? "-",
                    City = rpcity.Find(x.CityID).Name,
                    Region = rpregion.Find(x.RegionID).Name,
                    Address = x.Address ?? "-"
                }).ToList();
            }
            IPagedList<SupplierVM> list = model.ToPagedList(_page, 15);
            return View(list);
        }

        public ActionResult Add()
        {
            GetAllCitiesforAdding();
            return View();
        }

        private void GetAllCitiesforAdding()
        {
            ViewData["city"] = rpcity.GetAll().Select(x => new SelectListItem()
            {
                Value = x.ID.ToString(),
                Text = x.Name
            }).ToList();
            ViewData["region"] = rpregion.GetListWithQuery(x => x.CityID == 1).Select(x => new SelectListItem()
            {
                Value = x.ID.ToString(),
                Text = x.Name
            }).ToList();
        }

        private void GetAllCitiesforEditing(int cityId)
        {
            ViewData["city"] = rpcity.GetListWithQuery(x => x.ID == cityId).Select(x => new SelectListItem()
            {
                Value = x.ID.ToString(),
                Text = x.Name
            }).ToList();
            ViewData["region"] = rpregion.GetListWithQuery(x => x.CityID == cityId).Select(x => new SelectListItem()
            {
                Value = x.ID.ToString(),
                Text = x.Name
            }).ToList();
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
                    CityID = model.CityID,
                    RegionID = model.RegionID
                };
                rpsupplier.Add(entity);
                ViewBag.IslemDurum = EnumIslemDurum.Basarili;

            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
            }
            GetAllCitiesforAdding();
            return View(model);
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
                CityID = entity.CityID,
                RegionID = entity.RegionID
            };
            GetAllCitiesforEditing(entity.CityID);
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
                entity.CityID = model.CityID;
                entity.RegionID = model.RegionID;

                entity.UpdateDate = DateTime.Now;
                rpsupplier.SaveChanges();
                ViewBag.IslemDurum = EnumIslemDurum.Basarili;
            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
            }
            GetAllCitiesforEditing(model.CityID);
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            rpsupplier.Delete(id);
            return RedirectToAction("Index");
        }

        public JsonResult GetRegionsByID(int id)
        {
            var list = rpregion.GetListWithQuery(x => x.CityID == id).Select(x => new RegionVM()
            {
                ID = x.ID,
                Name = x.Name
            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}