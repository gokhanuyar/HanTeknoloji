using HanTeknoloji.Data.Models.Orm.Entity;
using HanTeknoloji.Web.Areas.Admin.Models.Barcode;
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
    public class AdminProductController : AdminBaseController
    {
        //
        public ActionResult Index(int? id, int? page, string searchString)
        {
            if (String.IsNullOrEmpty(searchString) && page == null && id == null)
            {
                Session.Remove("categoryid");
            }
            int categoryID = Convert.ToInt32(Session["categoryid"]);

            int _page = page ?? 1;

            List<ProductVM> model;
            if (id == null)
            {
                if (categoryID == 0)
                {
                    model = rpproduct.GetAll().OrderByDescending(x => x.AddDate).Select(x => new ProductVM
                    {
                        ID = x.ID,
                        SerialNumber = x.SerialNumber,
                        Count = x.Count,
                        TradeMark = rptrademark.Find(x.TradeMarkID).Name,
                        ProductModel = x.ProductModelID == 0 ? "Yok" : rpproductmodel.Find(x.ProductModelID).Name,
                        Payment = x.Payment,
                        UnitPrice = x.UnitPrice,
                    }).ToList();
                }
                else
                {
                    model = rpproduct.GetListWithQuery(x => x.CategoryID == categoryID).OrderByDescending(x => x.AddDate).Select(x => new ProductVM
                    {
                        ID = x.ID,
                        SerialNumber = x.SerialNumber,
                        Count = x.Count,
                        TradeMark = rptrademark.Find(x.TradeMarkID).Name,
                        ProductModel = x.ProductModelID == 0 ? "Yok" : rpproductmodel.Find(x.ProductModelID).Name,
                        UnitPrice = x.UnitPrice,
                        Payment = x.Payment
                    }).ToList();
                }
            }
            else
            {
                Session["categoryid"] = id;
                model = rpproduct.GetListWithQuery(x => x.CategoryID == id).OrderByDescending(x => x.AddDate).Select(x => new ProductVM
                {
                    ID = x.ID,
                    SerialNumber = x.SerialNumber,
                    Count = x.Count,
                    TradeMark = rptrademark.Find(x.TradeMarkID).Name,
                    ProductModel = x.ProductModelID == 0 ? "Yok" : rpproductmodel.Find(x.ProductModelID).Name,
                    UnitPrice = x.UnitPrice,
                    Payment = x.Payment
                }).ToList();
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                string search = searchString.ToLower();
                model = model.Where(x => x.TradeMark.ToLower().Contains(search) || x.ProductModel.ToLower().Contains(search) || x.SerialNumber.Contains(search) || x.Payment.ToLower().Contains(search)).ToList();
            }
            IPagedList<ProductVM> list = model.ToPagedList(_page, 15);
            GetAllCategories();
            return View(list);
        }

        public ActionResult Add()
        {
            GetDropdownItems(FirstTrademarkID());
            return View();
        }

        [HttpPost]
        public ActionResult Add(ProductVM model)
        {
            if (ModelState.IsValid)
            {
                Product entity = new Product()
                {
                    SerialNumber = model.SerialNumber,
                    TradeMarkID = model.TradeMarkID,
                    ProductModelID = model.ProductModelID,
                    Supplier = model.Supplier,
                    ColorID = model.ColorID,
                    UnitPrice = model.UnitPrice,
                    Count = model.Count,
                    CategoryID = model.CategoryID,
                    Payment = model.Payment,
                };
                rpproduct.Add(entity);
                ViewBag.IslemDurum = EnumIslemDurum.Basarili;
                ModelState.Clear();
            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
            }
            GetDropdownItems(FirstTrademarkID());
            return View();
        }

        public ActionResult Edit(int id)
        {
            var entity = rpproduct.Find(id);
            ProductVM model = new ProductVM()
            {
                ID = entity.ID,
                SerialNumber = entity.SerialNumber,
                TradeMarkID = entity.TradeMarkID,
                ProductModelID = entity.ProductModelID,
                Supplier = entity.Supplier,
                ColorID = entity.ColorID,
                UnitPrice = entity.UnitPrice,
                Count = entity.Count,
                CategoryID = entity.CategoryID,
                Payment = entity.Payment,
            };
            GetDropdownItems(entity.TradeMarkID);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProductVM model)
        {
            if (ModelState.IsValid)
            {
                Product entity = rpproduct.Find(model.ID);
                entity.SerialNumber = model.SerialNumber;
                entity.TradeMarkID = model.TradeMarkID;
                entity.ProductModelID = model.ProductModelID;
                entity.Supplier = model.Supplier;
                entity.ColorID = model.ColorID;
                entity.UnitPrice = model.UnitPrice;
                entity.Count = model.Count;
                entity.CategoryID = model.CategoryID;
                entity.Payment = model.Payment;
                entity.UpdateDate = DateTime.Now;
                rpproduct.SaveChanges();
                ViewBag.IslemDurum = EnumIslemDurum.Basarili;
            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
            }
            GetDropdownItems(model.TradeMarkID);
            return View(model);
        }

        private void GetDropdownItems(int id)
        {
            List<PaymentType> pt = new List<PaymentType>
            {
                new PaymentType {Type="Nakit" },
                new PaymentType {Type="Kredi Kartı" },
                new PaymentType {Type="Havale" },
                new PaymentType {Type="Vadeli" },
                new PaymentType {Type="Çek" }
            };
            ViewData["category"] = rpcategory.GetAll().Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.ID.ToString()
            }).ToList();
            ViewData["trademark"] = rptrademark.GetAll().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.ID.ToString()
            }).ToList();
            ViewData["payment"] = pt.Select(x => new SelectListItem()
            {
                Text = x.Type,
                Value = x.Type
            }).ToList();

            ViewData["color"] = rpcolor.GetAll().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.ID.ToString()
            }).ToList();

            SelectListItem item = new SelectListItem() { Value = "0", Text = "-" };
            var modelList = rpproductmodel.GetListWithQuery(x => x.TradeMarkID == id).Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.ID.ToString()
            }).ToList();
            modelList.Add(item);
            ViewData["productmodel"] = modelList;
        }

        private void GetAllCategories()
        {
            ViewData["category"] = rpcategory.GetAll().Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.ID.ToString()
            }).ToList();
        }

        public JsonResult GetProductModelsByID(int id)
        {
            var list = rpproductmodel.GetListWithQuery(x => x.TradeMarkID == id).Select(x => new ProductModelVM()
            {
                ID = x.ID,
                Name = x.Name
            }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            rpproduct.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult SingleBarCode()
        {
            barcodecs objbar = new barcodecs();
            byte[] url = objbar.getBarcodeImage("123456", "");
            BarCodeVM model = new BarCodeVM();
            model.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])url);

            return View(model);
        }
    }
}