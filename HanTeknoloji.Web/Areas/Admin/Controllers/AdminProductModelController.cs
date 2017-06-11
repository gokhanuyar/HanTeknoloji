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
    [Authorize]
    public class AdminProductModelController : AdminBaseController
    {
        public ActionResult Index(int? id, int? page, string searchString)
        {
            if (String.IsNullOrEmpty(searchString) && page == null && id == null)
            {
                Session.Remove("trademarkid");
            }
            int tradeMarkID = Convert.ToInt32(Session["trademarkid"]);

            int _page = page ?? 1;

            List<ProductModelVM> model;
            if (id == null)
            {
                if (tradeMarkID == 0)
                {
                    model = rpproductmodel.GetAll().OrderByDescending(x => x.AddDate).Select(x => new ProductModelVM
                    {
                        ID = x.ID,
                        Name = x.Name,
                        BarcodeValue = x.BarcodeValue
                    }).ToList();
                }
                else
                {
                    model = rpproductmodel.GetListWithQuery(x => x.TradeMarkID == tradeMarkID).OrderByDescending(x => x.AddDate).Select(x => new ProductModelVM
                    {
                        ID = x.ID,
                        Name = x.Name
                    }).ToList();
                }
            }
            else
            {
                Session["trademarkid"] = id;
                model = rpproductmodel.GetListWithQuery(x => x.TradeMarkID == id).OrderByDescending(x => x.AddDate).Select(x => new ProductModelVM
                {
                    ID = x.ID,
                    Name = x.Name
                }).ToList();
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                string search = searchString.ToLower();
                model = model.Where(x => x.Name.ToLower().Contains(search)).ToList();
            }
            IPagedList<ProductModelVM> list = model.ToPagedList(_page, 15);
            GetAllTradeMarks();
            return View(list);
        }

        private void GetAllTradeMarks()
        {
            ViewData["trademark"] = rptrademark.GetAll().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.ID.ToString()
            }).ToList();
        }

        public ActionResult Add()
        {
            GetAllTradeMarks();
            return View();
        }

        [HttpPost]
        public ActionResult Add(ProductModelVM model)
        {
            if (ModelState.IsValid)
            {
                ProductModel entity = new ProductModel
                {
                    Name = model.Name,
                    TradeMarkID = model.TradeMarkID,
                    BarcodeValue = model.BarcodeValue
                };
                rpproductmodel.Add(entity);
                ViewBag.IslemDurum = EnumIslemDurum.Basarili;

            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
            }
            GetAllTradeMarks();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var entity = rpproductmodel.Find(id);
            ProductModelVM model = new ProductModelVM
            {
                ID = entity.ID,
                Name = entity.Name,
                TradeMarkID = entity.TradeMarkID,
                BarcodeValue = entity.BarcodeValue
            };
            GetAllTradeMarks();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProductModelVM model)
        {
            if (ModelState.IsValid)
            {
                ProductModel entity = rpproductmodel.Find(model.ID);
                entity.Name = model.Name;
                entity.TradeMarkID = model.TradeMarkID;
                entity.BarcodeValue = model.BarcodeValue;
                entity.UpdateDate = DateTime.Now;
                rpproductmodel.SaveChanges();
                ViewBag.IslemDurum = EnumIslemDurum.Basarili;
            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
            }
            GetAllTradeMarks();
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            rpproductmodel.Delete(id);
            return RedirectToAction("Index");
        }
    }
}