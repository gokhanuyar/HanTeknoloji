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
    public class AdminColorController : AdminBaseController
    {
        public ActionResult Index(int? page)
        {
            int _page = page ?? 1;
            var list = rpcolor.GetAll().Select(x => new ColorVM
            {
                Name = x.Name,
                ID = x.ID,
                BarcodeValue = x.BarcodeValue
            }).ToPagedList(_page, 20);
            return View(list);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(ColorVM model)
        {
            if (ModelState.IsValid)
            {
                var color = rpcolor.FirstOrDefault(x => x.Name.ToLower() == model.Name.ToLower() || x.BarcodeValue == model.BarcodeValue);
                if (color != null)
                {
                    ViewBag.IslemDurum = color.Name.ToLower() == model.Name.ToLower() ? EnumIslemDurum.IsimMevcut : EnumIslemDurum.BarkodMevcut;
                }
                else
                {
                    Color entity = new Color
                    {
                        Name = model.Name,
                        BarcodeValue = model.BarcodeValue
                    };
                    rpcolor.Add(entity);
                    ViewBag.IslemDurum = EnumIslemDurum.Basarili;
                }
            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var entity = rpcolor.Find(id);
            ColorVM model = new ColorVM
            {
                ID = entity.ID,
                Name = entity.Name,
                BarcodeValue = entity.BarcodeValue
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ColorVM model)
        {
            if (ModelState.IsValid)
            {
                var color = rpcolor.FirstOrDefault(x => (x.Name.ToLower() == model.Name.ToLower() || x.BarcodeValue == model.BarcodeValue) && x.ID != model.ID);
                if (color != null)
                {
                    ViewBag.IslemDurum = color.Name.ToLower() == model.Name.ToLower() ? EnumIslemDurum.IsimMevcut : EnumIslemDurum.BarkodMevcut;
                }
                else
                {
                    Color entity = rpcolor.Find(model.ID);
                    entity.Name = model.Name;
                    entity.BarcodeValue = model.BarcodeValue;
                    entity.UpdateDate = DateTime.Now;
                    rpcolor.SaveChanges();
                    ViewBag.IslemDurum = EnumIslemDurum.Basarili;
                }
            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            rpcolor.Delete(id);
            return RedirectToAction("Index");
        }
    }
}