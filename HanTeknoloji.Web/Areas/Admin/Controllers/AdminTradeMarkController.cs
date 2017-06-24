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
    public class AdminTradeMarkController : AdminBaseController
    {
        public ActionResult Index(int? page, string searchString)
        {
            int _page = page ?? 1;

            List<TradeMarkVM> model;
            if (string.IsNullOrEmpty(searchString))
            {
                model = rptrademark.GetAll().OrderByDescending(x => x.AddDate).Select(x => new TradeMarkVM
                {
                    ID = x.ID,
                    Name = x.Name,
                    BarcodeValue = x.BarcodeValue
                }).ToList();
            }
            else
            {
                string search = searchString.ToLower();
                model = rptrademark.GetListWithQuery(x => x.Name.ToLower().Contains(search)).OrderByDescending(x => x.AddDate).Select(x => new TradeMarkVM
                {
                    ID = x.ID,
                    Name = x.Name
                }).ToList();
            }
            IPagedList<TradeMarkVM> list = model.ToPagedList(_page, 15);
            return View(list);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(TradeMarkVM model)
        {
            if (ModelState.IsValid)
            {
                var trademark = rptrademark.FirstOrDefault(x => x.Name.ToLower() == model.Name.ToLower() || x.BarcodeValue == model.BarcodeValue);
                if (trademark != null)
                {
                    ViewBag.IslemDurum = trademark.Name.ToLower() == model.Name.ToLower() ? EnumIslemDurum.IsimMevcut : EnumIslemDurum.BarkodMevcut;
                }
                else
                {
                    TradeMark entity = new TradeMark
                    {
                        Name = model.Name,
                        BarcodeValue = model.BarcodeValue
                    };
                    rptrademark.Add(entity);
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
            var entity = rptrademark.Find(id);
            TradeMarkVM model = new TradeMarkVM
            {
                ID = entity.ID,
                Name = entity.Name,
                BarcodeValue = entity.BarcodeValue
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(TradeMarkVM model)
        {
            if (ModelState.IsValid)
            {
                var trademark = rptrademark.FirstOrDefault(x => (x.Name.ToLower() == model.Name.ToLower() || x.BarcodeValue == model.BarcodeValue) && x.ID != model.ID);
                if (trademark != null)
                {
                    ViewBag.IslemDurum = trademark.Name.ToLower() == model.Name.ToLower() ? EnumIslemDurum.IsimMevcut : EnumIslemDurum.BarkodMevcut;
                }
                else
                {
                    TradeMark entity = rptrademark.Find(model.ID);
                    entity.Name = model.Name;
                    entity.BarcodeValue = model.BarcodeValue;
                    entity.UpdateDate = DateTime.Now;
                    rptrademark.SaveChanges();
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
            rptrademark.Delete(id);
            return RedirectToAction("Index");
        }
    }
}