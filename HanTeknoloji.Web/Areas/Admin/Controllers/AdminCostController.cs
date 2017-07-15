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
    public class AdminCostController : AdminBaseController
    {
        public ActionResult TechnicalServiceList(int? page)
        {
            int _page = page ?? 1;
            var model = rptechservicecost.GetAll().OrderByDescending(x => x.CostDate).Select(x => new TechnicalServiceCostVM
            {
                ID = x.ID,
                Cost = x.Cost,
                CostDateString = x.CostDate.ToLongDateString()
            }).ToPagedList(_page, 15);
            return View(model);
        }

        public ActionResult TechnicalService()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TechnicalService(TechnicalServiceCostVM model)
        {
            if (ModelState.IsValid)
            {
                if (rptechservicecost.Any(x => x.CostDate == model.CostDate))
                {
                    ViewBag.IslemDurum = EnumIslemDurum.IsimMevcut;
                }
                else
                {
                    var entity = new TechnicalServiceCost
                    {
                        Cost = model.Cost,
                        CostDate = model.CostDate
                    };
                    rptechservicecost.Add(entity);
                    ViewBag.IslemDurum = EnumIslemDurum.Basarili;
                }
            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
            }
            return View(model);
        }

        public ActionResult EditTechnical(int id)
        {
            var entity = rptechservicecost.Find(id);
            TechnicalServiceCostVM model = new TechnicalServiceCostVM
            {
                ID = entity.ID,
                Cost = entity.Cost,
                CostDateString = entity.CostDate.ToLongDateString()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditTechnical(TechnicalServiceCostVM model)
        {
            if (ModelState.IsValid)
            {
                var entity = rptechservicecost.Find(model.ID);
                entity.Cost = model.Cost;
                rptechservicecost.SaveChanges();
                ViewBag.IslemDurum = EnumIslemDurum.Basarili;
            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
            }
            return View(model);
        }

        public ActionResult DeleteTechnical(int id)
        {
            rptechservicecost.Delete(id);
            return RedirectToAction("TechnicalServiceList");
        }
    }
}