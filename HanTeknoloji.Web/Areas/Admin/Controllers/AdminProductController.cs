using HanTeknoloji.Data.Models.Orm.Entity;
using HanTeknoloji.Web.Areas.Admin.Models.Attributes;
using HanTeknoloji.Web.Areas.Admin.Models.Barcode;
using HanTeknoloji.Web.Areas.Admin.Models.Services;
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
    public class AdminProductController : AdminBaseController
    {
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
                        ProductModel = x.ProductModelID == 0 ? "Yok" : rpproductmodel.Find(x.ProductModelID).Name
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
                        ProductModel = x.ProductModelID == 0 ? "Yok" : rpproductmodel.Find(x.ProductModelID).Name
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
                    ProductModel = x.ProductModelID == 0 ? "Yok" : rpproductmodel.Find(x.ProductModelID).Name
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
            return View(new ProductVM());
        }

        [HttpPost]
        public ActionResult Add(ProductVM model)
        {
            DateTime date = Convert.ToDateTime(model.ExpiryDate);
            if (ModelState.IsValid)
            {
                if (model.SerialNumber == null)
                {
                    //string category = rpcategory.Find(model.CategoryID).BarcodeValue;
                    string trademark = rptrademark.Find(model.TradeMarkID).BarcodeValue;
                    string proModel = model.ProductModelID == 0 ? "000" : rpproductmodel.Find(model.ProductModelID).BarcodeValue;
                    string color = model.ColorID != 0 ? rpcolor.Find(model.ColorID).BarcodeValue : "0";
                    model.SerialNumber = trademark + proModel + color;
                }
                var product = rpproduct.FirstOrDefault(x => x.SerialNumber == model.SerialNumber);
                if (product != null)
                {
                    product.Count += model.Count;
                    product.UnitPrice = model.UnitPrice;
                    product.UnitSalePrice = model.UnitSalePrice;
                    rpproduct.SaveChanges();
                    SaveOthers(product.ID, model);
                }
                else
                {
                    Product entity = new Product()
                    {
                        SerialNumber = model.SerialNumber,
                        TradeMarkID = model.TradeMarkID,
                        ProductModelID = model.ProductModelID,
                        SupplierID = model.SupplierID,
                        ColorID = model.ColorID,
                        UnitPrice = model.UnitPrice,
                        UnitSalePrice = model.UnitSalePrice,
                        Count = model.Count,
                        CategoryID = model.CategoryID,
                        KDV = model.KDV
                    };
                    rpproduct.Add(entity);
                    SaveOthers(entity.ID, model);
                }

                ViewBag.IslemDurum = EnumIslemDurum.Basarili;
                ModelState.Clear();
                GetDropdownItems(FirstTrademarkID());
                return View(new ProductVM());
            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
                GetDropdownItems(FirstTrademarkID());
                return View(model);
            }
        }

        public ActionResult AddWithBarcode(string id)
        {
            ProductVM model = new ProductVM();
            if (!string.IsNullOrEmpty(id))
            {
                var entity = rpproduct.FirstOrDefault(x => x.SerialNumber == id);
                if (entity != null)
                {
                    model.ID = entity.ID;
                    model.SerialNumber = entity.SerialNumber;
                    model.TradeMarkID = entity.TradeMarkID;
                    model.ProductModelID = entity.ProductModelID;
                    model.SupplierID = entity.SupplierID;
                    model.ColorID = entity.ColorID;
                    model.UnitPrice = entity.UnitPrice;
                    model.CategoryID = entity.CategoryID;
                    model.KDV = entity.KDV;
                    model.UnitSalePrice = entity.UnitSalePrice;
                    GetDropdownItems(entity.TradeMarkID);
                    return View(model);
                }
                else
                {
                    GetDropdownItems(FirstTrademarkID());
                    return View(new ProductVM());
                }
            }
            else
            {
                GetDropdownItems(FirstTrademarkID());
                return View(new ProductVM());
            }
        }

        [HttpPost]
        public ActionResult AddWithBarcode(ProductVM model)
        {
            DateTime date = Convert.ToDateTime(model.ExpiryDate);
            if (ModelState.IsValid)
            {
                if (model.SerialNumber == null)
                {
                    //string category = rpcategory.Find(model.CategoryID).BarcodeValue;
                    string trademark = rptrademark.Find(model.TradeMarkID).BarcodeValue;
                    string proModel = model.ProductModelID == 0 ? "000" : rpproductmodel.Find(model.ProductModelID).BarcodeValue;
                    string color = model.ColorID != 0 ? rpcolor.Find(model.ColorID).BarcodeValue : "0";
                    model.SerialNumber = trademark + proModel + color;
                }
                var product = rpproduct.FirstOrDefault(x => x.SerialNumber == model.SerialNumber);
                if (product != null)
                {
                    product.Count += model.Count;
                    product.UnitPrice = model.UnitPrice;
                    product.UnitSalePrice = model.UnitSalePrice;
                    rpproduct.SaveChanges();
                    SaveOthers(product.ID, model);
                }
                else
                {
                    Product entity = new Product()
                    {
                        SerialNumber = model.SerialNumber,
                        TradeMarkID = model.TradeMarkID,
                        ProductModelID = model.ProductModelID,
                        SupplierID = model.SupplierID,
                        ColorID = model.ColorID,
                        UnitPrice = model.UnitPrice,
                        UnitSalePrice = model.UnitSalePrice,
                        Count = model.Count,
                        CategoryID = model.CategoryID,
                        KDV = model.KDV
                    };
                    rpproduct.Add(entity);
                    SaveOthers(entity.ID, model);
                }

                ViewBag.IslemDurum = EnumIslemDurum.Basarili;
                ModelState.Clear();
                GetDropdownItems(FirstTrademarkID());
                return View(new ProductVM());
            }
            else
            {
                ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
                GetDropdownItems(FirstTrademarkID());
                return View(model);
            }
        }

        private void SaveOthers(int id, ProductVM model)
        {
            var paymentInfo = new PaymentInfo
            {
                BankName = model.BankName,
                BankCartName = model.BankCartName,
                CartNumber = model.CartNumber,
                Payment = model.Payment,
                CheckNumber = model.CheckNumber,
                ExpiryDate = model.ExpiryDate == null ? DateTime.Now : Convert.ToDateTime(model.ExpiryDate),
                ProductID = id,
                UnitPrice = model.UnitPrice,
                Count = model.Count,
                BuyingCount = model.Count,
                SupplierID = model.SupplierID
            };
            if (model.CategoryID == 6 || model.CategoryID == 7)
            {
                paymentInfo.IMEICount = model.Count;
            }
            rppaymentinfo.Add(paymentInfo);
            if (model.Payment == "Vadeli")
            {
                var supplierExpiry = new SupplierExpiry
                {
                    ExpiryDate = model.ExpiryDate == null ? DateTime.Now : Convert.ToDateTime(model.ExpiryDate),
                    PaidPrice = Convert.ToDecimal(model.PaidPrice),
                    ProductID = id,
                    ProductCount = model.Count,
                    SupplierID = model.SupplierID,
                    TotalBuyingPrice = Calculate(model)
                };
                rpsupplierexpiry.Add(supplierExpiry);
                if (model.PaidPrice != 0)
                {
                    var expiryPayment = new ExpiryPayment
                    {
                        AdminUserID = UserID(),
                        PersonID = model.SupplierID,
                        Price = model.PaidPrice
                    };
                    rpexpirypayment.Add(expiryPayment);
                }
                ExpiryService.SetSupplierExpiry(supplierExpiry);
            }
        }

        private decimal Calculate(ProductVM model)
        {
            return model.Count * model.UnitPrice;
        }

        public ActionResult Edit(int id)
        {
            var entity = rpproduct.Find(id);
            var paymentInfo = rppaymentinfo.FirstOrDefault(x => x.ProductID == id);
            ProductVM model = new ProductVM()
            {
                ID = entity.ID,
                SerialNumber = entity.SerialNumber,
                TradeMarkID = entity.TradeMarkID,
                ProductModelID = entity.ProductModelID,
                SupplierID = entity.SupplierID,
                ColorID = entity.ColorID,
                UnitPrice = entity.UnitPrice,
                Count = entity.Count,
                CategoryID = entity.CategoryID,
                KDV = entity.KDV,
                UnitSalePrice = entity.UnitSalePrice,
                Payment = paymentInfo.Payment,
                BankName = paymentInfo.BankName,
                BankCartName = paymentInfo.BankCartName,
                CartNumber = paymentInfo.CartNumber,
                CheckNumber = paymentInfo.CheckNumber,
                ExpiryDate = string.Format("{0:yyyy-MM-dd}", paymentInfo.ExpiryDate)
            };
            GetDropdownItems(entity.TradeMarkID);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProductVM model)
        {
            if (ModelState.IsValid)
            {
                var entity = rpproduct.Find(model.ID);
                entity.SerialNumber = model.SerialNumber;
                entity.TradeMarkID = model.TradeMarkID;
                entity.ProductModelID = model.ProductModelID;
                entity.SupplierID = model.SupplierID;
                entity.ColorID = model.ColorID;
                entity.UnitPrice = model.UnitPrice;
                entity.Count = model.Count;
                entity.CategoryID = model.CategoryID;
                entity.KDV = model.KDV;
                entity.UnitSalePrice = model.UnitSalePrice;
                entity.UpdateDate = DateTime.Now;
                rpproduct.SaveChanges();

                var paymentInfo = rppaymentinfo.FirstOrDefault(x => x.ProductID == entity.ID);
                paymentInfo.Payment = model.Payment;
                paymentInfo.BankName = model.BankName;
                paymentInfo.BankCartName = model.BankCartName;
                paymentInfo.CartNumber = model.CartNumber;
                paymentInfo.CheckNumber = model.CheckNumber;
                paymentInfo.ExpiryDate = Convert.ToDateTime(model.ExpiryDate);
                paymentInfo.UnitPrice = model.UnitPrice;
                paymentInfo.Count = model.Count;
                paymentInfo.BuyingCount = model.Count;
                paymentInfo.UpdateDate = DateTime.Now;
                rppaymentinfo.SaveChanges();

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

            ViewData["supplier"] = rpsupplier.GetAll().Select(x => new SelectListItem()
            {
                Text = x.CompanyName,
                Value = x.ID.ToString()
            }).ToList();

            //SelectListItem item = new SelectListItem() { Value = "0", Text = "-" };
            var modelList = rpproductmodel.GetListWithQuery(x => x.TradeMarkID == id).Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.ID.ToString()
            }).ToList() ?? new List<SelectListItem>();
            //modelList.Add(item);
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

        //public ActionResult Delete(int id)
        //{
        //    rpproduct.Delete(id);
        //    return RedirectToAction("Index");
        //}

        public ActionResult BarCode(int id)
        {
            var product = rpproduct.Find(id);
            string trademark = rptrademark.Find(product.TradeMarkID).Name;
            string promodel = rpproductmodel.Find(product.ProductModelID).Name;

            barcodecs objbar = new barcodecs();
            byte[] url = objbar.getBarcodeImage(product.SerialNumber, trademark + " " + promodel);
            BarCodeModel model = new BarCodeModel();
            model.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])url);

            return View(model);
        }

        public JsonResult Detail(int id)
        {
            var product = rpproduct.Find(id);
            var supplier = rpsupplier.Find(product.SupplierID);
            return Json(new
            {
                buyprice = product.UnitPrice,
                saleprice = product.UnitSalePrice,
                KDV = product.KDV * 100,
                supplier = supplier.CompanyName
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IMEI(int? SupplierID)
        {
            int count = 0;
            ViewBag.supplierId = 0;
            ViewData["product"] = new List<SelectListItem>();
            if (SupplierID == null)
            {
                var paymentInfoList = rppaymentinfo.GetListWithQuery(x => x.IMEICount > 0);
                count = paymentInfoList.Sum(x => x.IMEICount);
            }
            else
            {
                var paymentInfoList = rppaymentinfo.GetListWithQuery(x => x.IMEICount > 0 && x.SupplierID == SupplierID);
                count = paymentInfoList.Sum(x => x.IMEICount);
                ViewBag.supplierId = SupplierID;
                GetProducts(SupplierID, paymentInfoList);
            }

            ViewBag.count = count;
            GetAllSuppliers();
            return View();
        }

        [HttpPost]
        public ActionResult IMEI(ImeiVM model)
        {
            ViewData["product"] = new List<SelectListItem>();
            if (ModelState.IsValid)
            {
                if (rpimei.Any(x => x.IMEINumber == model.IMEINumber))
                {
                    ViewBag.IslemDurum = EnumIslemDurum.IsimMevcut;
                    var paymentInfoList = rppaymentinfo.GetListWithQuery(x => x.IMEICount > 0 && x.SupplierID == model.SupplierID);
                    ViewBag.count = model.ProductID != 0 ? paymentInfoList.Where(x => x.ProductID == model.ProductID).Sum(x => x.IMEICount) : paymentInfoList.Sum(x => x.IMEICount);
                    ViewBag.supplierId = model.SupplierID;
                    GetProducts(model.SupplierID, paymentInfoList);
                    GetAllSuppliers();
                    return View(model);
                }
                else
                {
                    var paymentInfo = rppaymentinfo.FirstOrDefault(x => x.IMEICount > 0 && x.SupplierID == model.SupplierID && x.ProductID == model.ProductID);
                    paymentInfo.IMEICount--;

                    var imei = new IMEI
                    {
                        IMEINumber = model.IMEINumber,
                        SupplierID = model.SupplierID,
                        PaymentInfoID = paymentInfo.ID,
                        ProductID = model.ProductID
                    };
                    rpimei.Add(imei);
                    rppaymentinfo.SaveChanges();

                    var paymentInfoList = rppaymentinfo.GetListWithQuery(x => x.IMEICount > 0 && x.SupplierID == model.SupplierID && x.ProductID == model.ProductID);
                    ViewBag.count = paymentInfoList.Sum(x => x.IMEICount);
                    ViewBag.supplierId = model.SupplierID;
                    ViewBag.IslemDurum = EnumIslemDurum.Basarili;

                    var paymentInfoListforProducts = rppaymentinfo.GetListWithQuery(x => x.IMEICount > 0 && x.SupplierID == model.SupplierID);
                    GetProducts(model.SupplierID, paymentInfoListforProducts);

                    GetAllSuppliers();
                    return View(model);
                }
            }
            else
            {
                var paymentInfoList = rppaymentinfo.GetListWithQuery(x => x.IMEICount > 0 && x.SupplierID == model.SupplierID);
                ViewBag.count = model.ProductID != 0 ? paymentInfoList.Where(x => x.ProductID == model.ProductID).Sum(x => x.IMEICount) : paymentInfoList.Sum(x => x.IMEICount);
                ViewBag.supplierId = model.SupplierID;
                ViewBag.IslemDurum = EnumIslemDurum.ValidationHata;
                GetProducts(model.SupplierID, paymentInfoList);
                GetAllSuppliers();
                return View(model);
            }
        }

        private void GetAllSuppliers()
        {
            ViewData["supplier"] = rpsupplier.GetAll().Select(x => new SelectListItem
            {
                Text = x.CompanyName,
                Value = x.ID.ToString()
            });
        }

        private void GetProducts(int? supplierID, List<PaymentInfo> list)
        {
            var productList = new List<Product>();
            foreach (var info in list)
            {
                var product = rpproduct.Find(info.ProductID);
                productList.Add(product);
            }
            ViewData["product"] = productList.Select(x => new SelectListItem
            {
                Text = rptrademark.Find(x.TradeMarkID).Name + " " + rpproductmodel.Find(x.ProductModelID).Name,
                Value = x.ID.ToString()
            });
        }

        public JsonResult GetImeiCount(int productId, int supplierId)
        {
            var count = rppaymentinfo.GetListWithQuery(x => x.IMEICount > 0 && x.SupplierID == supplierId && x.ProductID == productId).Sum(x => x.IMEICount);
            return Json(count, JsonRequestBehavior.AllowGet);
        }
    }
}