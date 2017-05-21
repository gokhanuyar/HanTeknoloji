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
    public class AdminCustomerController : AdminBaseController
    {
        public ActionResult Index(int? page, string searchString)
        {
            int _page = page ?? 1;

            List<CustomerVM> model;
            if (string.IsNullOrEmpty(searchString))
            {
                model = rpcustomer.GetAll().OrderByDescending(x => x.AddDate).Select(x => new CustomerVM
                {
                    ID = x.ID,
                    TCNo = x.TCNo,
                    Name = x.Name,
                    City = rpcity.Find(x.CityID).Name,
                    Region = rpregion.Find(x.RegionID).Name,
                    Address = x.Address ?? "-",
                    Phone = x.Phone
                }).ToList();
            }
            else
            {
                string search = searchString.ToLower();
                model = rpcustomer.GetListWithQuery(x => x.Name.ToLower().Contains(search)).OrderByDescending(x => x.AddDate).Select(x => new CustomerVM
                {
                    ID = x.ID,
                    TCNo = x.TCNo,
                    Name = x.Name,
                    City = rpcity.Find(x.CityID).Name,
                    Region = rpregion.Find(x.RegionID).Name,
                    Address = x.Address ?? "-",
                    Phone = x.Phone
                }).ToList();
            }
            IPagedList<CustomerVM> list = model.ToPagedList(_page, 15);
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
        public ActionResult Add(CustomerVM model)
        {
            if (ModelState.IsValid)
            {
                bool tcKontrol = false;
                model.TCNo = model.TCNo == null ? "" : model.TCNo;
                if (model.TCNo.Length == 11)
                    tcKontrol = TCNoKontrolu(model.TCNo);


                if (tcKontrol || model.TCNo == "")
                {
                    Customer entity = new Customer
                    {
                        Name = model.Name,
                        Address = model.Address,
                        CityID = model.CityID,
                        RegionID = model.RegionID,
                        TCNo = model.TCNo,
                        Phone = model.Phone,
                        TaxNumber = model.TaxNumber,
                        TaxOffice = model.TaxOffice,
                        IsPerson = model.IsPerson
                    };
                    rpcustomer.Add(entity);
                    ViewBag.IslemDurum = EnumIslemDurum.Basarili;
                }
                else
                {
                    ViewBag.IslemDurum = EnumIslemDurum.YanlısTCNo;
                }
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
            var entity = rpcustomer.Find(id);
            CustomerVM model = new CustomerVM
            {
                ID = entity.ID,
                Name = entity.Name,
                Address = entity.Address,
                CityID = entity.CityID,
                RegionID = entity.RegionID,
                TCNo = entity.TCNo,
                Phone = entity.Phone,
                TaxNumber = entity.TaxNumber,
                TaxOffice = entity.TaxOffice
            };
            GetAllCitiesforEditing(entity.CityID);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CustomerVM model)
        {
            if (ModelState.IsValid)
            {
                bool tcKontrol = false;
                model.TCNo = model.TCNo == null ? "" : model.TCNo;
                if (model.TCNo.Length == 11)
                    tcKontrol = TCNoKontrolu(model.TCNo);
                if (tcKontrol || model.TCNo == "")
                {
                    Customer entity = rpcustomer.Find(model.ID);
                    entity.Name = model.Name;
                    entity.Address = model.Address;
                    entity.CityID = model.CityID;
                    entity.RegionID = model.RegionID;
                    entity.TCNo = model.TCNo;
                    entity.Phone = model.Phone;
                    entity.TaxNumber = model.TaxNumber;
                    entity.TaxOffice = model.TaxOffice;
                    entity.UpdateDate = DateTime.Now;
                    entity.IsPerson = model.IsPerson;

                    rpcustomer.SaveChanges();
                    ViewBag.IslemDurum = EnumIslemDurum.Basarili;
                }
                else
                {
                    ViewBag.IslemDurum = EnumIslemDurum.YanlısTCNo;
                }
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
            rpcustomer.Delete(id);
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

        public static bool TCNoKontrolu(string TCNo)
        {
            int[] TC = new int[11];

            for (int i = 0; i < 11; i++)
            {
                string a = TCNo[i].ToString();
                TC[i] = Convert.ToInt32(a);
            }
            int tekler = 0;
            int ciftler = 0;

            for (int k = 0; k < 9; k++)
            {
                if (k % 2 == 0)
                    tekler += TC[k];
                else if (k % 2 != 0)
                    ciftler += TC[k];
            }

            int t1 = (tekler * 3) + ciftler;
            int c1 = (10 - (t1 % 10)) % 10;
            int t2 = c1 + ciftler;
            int t3 = (t2 * 3) + tekler;
            int c2 = (10 - (t3 % 10)) % 10;
            if (c1 == TC[9] && c2 == TC[10])
                return true;
            else
                return false;
        }
    }
}