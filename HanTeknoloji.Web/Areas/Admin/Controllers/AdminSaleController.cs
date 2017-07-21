using HanTeknoloji.Data.Models.Orm.Entity;
using HanTeknoloji.Web.Areas.Admin.Models.Dto;
using HanTeknoloji.Web.Areas.Admin.Models.Services;
using HanTeknoloji.Web.Areas.Admin.Models.Types.Enums;
using HanTeknoloji.Web.Areas.Admin.Models.Types.Messages;
using HanTeknoloji.Web.Areas.Admin.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HanTeknoloji.Web.Areas.Admin.Controllers
{
    public class AdminSaleController : AdminBaseController
    {
        public ActionResult Index()
        {
            SaleWrapVM model = new SaleWrapVM();
            model.Cart = new CartVM();
            model.Customer = new CustomerVM();
            model.Cart.ProductList = new List<ProductVM>();
            if (Session["Sepet"] != null)
            {
                model.Cart = (CartVM)Session["Sepet"];
            }
            GetCustomers();
            GetAllCitiesforAdding();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string BarcodeNumber)
        {
            GetCustomers();
            GetAllCitiesforAdding();
            SaleWrapVM model = new SaleWrapVM();
            model.Cart = new CartVM();
            model.Customer = new CustomerVM();
            model.Cart.ProductList = new List<ProductVM>();
            if (!string.IsNullOrEmpty(BarcodeNumber))
            {
                var product = rpproduct.FirstOrDefault(x => x.SerialNumber == BarcodeNumber && x.Count != 0);
                if (product != null)
                {
                    if (product.Count != 0)
                    {
                        product.Count -= 1;
                        rpproduct.SaveChanges();

                        bool varMi = false;

                        if (Session["Sepet"] != null)
                        {
                            model.Cart = (CartVM)Session["Sepet"];
                            foreach (var item in model.Cart.ProductList)
                            {
                                if (item.SerialNumber == BarcodeNumber)
                                {
                                    item.SaleCount++;
                                    item.Count = product.Count;
                                    varMi = true;
                                    item.UnitSalePrice = item.UnitPrice * item.SaleCount;
                                    item.KdvPrice = Math.Round(item.UnitSalePrice * item.KDV, 4);
                                    item.TotalPrice = 0;
                                    model.Cart.TotalSalePrice += item.TotalPrice / item.SaleCount;
                                    if (item.CategoryID == 6 || item.CategoryID == 7)
                                    {
                                        model.Cart.PhoneCount++;
                                        model.Cart.ImeiCount = AddedImeiCount(model.Cart.ProductList);
                                    }
                                    break;
                                }
                            }
                        }

                        if (!varMi)
                        {
                            ProductVM pro = new ProductVM()
                            {
                                ID = product.ID,
                                SerialNumber = product.SerialNumber,
                                TradeMark = rptrademark.Find(product.TradeMarkID).Name,
                                ProductModel = product.ProductModelID == 0 ? "" : rpproductmodel.Find(product.ProductModelID).Name,
                                Color = rpcolor.Find(product.ColorID).Name,
                                UnitSalePrice = product.UnitSalePrice,//satış fiyatı değişken
                                UnitPrice = product.UnitSalePrice,//satış fiyatı
                                //UnitBuyPrice = product.UnitPrice,//alış fiyatı
                                Count = product.Count,
                                CategoryID = product.CategoryID,
                                SaleCount = 1,
                                KDV = product.KDV,
                                KdvPrice = Math.Round((product.UnitSalePrice * product.KDV), 4),
                                ImeiList = new List<int>()
                            };

                            model.Cart.ProductList.Add(pro);
                            model.Cart.TotalSalePrice += pro.TotalPrice;
                            if (pro.CategoryID == 6 || pro.CategoryID == 7)
                            {
                                model.Cart.PhoneCount++;
                                model.Cart.ImeiCount = AddedImeiCount(model.Cart.ProductList);
                            }

                        }
                        Session["Sepet"] = model.Cart;
                    }
                    else
                    {
                        if (Session["Sepet"] != null)
                        {
                            model.Cart = (CartVM)Session["Sepet"];
                        }
                        ViewBag.IslemDurum = EnumIslemDurum.StokYetersiz;
                        return View(model);
                    }
                }
                else
                {
                    if (Session["Sepet"] != null)
                    {
                        model.Cart = (CartVM)Session["Sepet"];
                    }
                    ViewBag.IslemDurum = EnumIslemDurum.UrunYok;
                    return View(model);
                }
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var product = rpproduct.Find(id);
            var sepet = (CartVM)Session["Sepet"];
            var selectedItem = sepet.ProductList.FirstOrDefault(x => x.ID == id);
            sepet.TotalSalePrice -= selectedItem.TotalPrice;

            product.Count += selectedItem.SaleCount;
            rpproduct.SaveChanges();

            if (selectedItem.CategoryID == 6 || selectedItem.CategoryID == 7)
            {
                sepet.PhoneCount -= selectedItem.SaleCount;
            }

            sepet.ProductList.Remove(selectedItem);
            sepet.ImeiCount = AddedImeiCount(sepet.ProductList);
            Session["Sepet"] = sepet;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeQuantity(int id, int quantity)
        {
            var product = rpproduct.Find(id);
            var sepet = (CartVM)Session["Sepet"];
            var selectedItem = sepet.ProductList.FirstOrDefault(x => x.ID == id);

            int oldCount = product.Count + selectedItem.SaleCount;
            product.Count = oldCount - quantity;
            rpproduct.SaveChanges();

            if (selectedItem.CategoryID == 6 || selectedItem.CategoryID == 7)
            {
                sepet.PhoneCount -= selectedItem.SaleCount;
                sepet.PhoneCount += quantity;
            }

            selectedItem.Count = product.Count;
            selectedItem.SaleCount = quantity;
            selectedItem.UnitSalePrice = selectedItem.UnitPrice * selectedItem.SaleCount;
            selectedItem.KdvPrice = Math.Round(selectedItem.UnitSalePrice * selectedItem.KDV, 4);
            selectedItem.TotalPrice = 0;
            sepet.TotalSalePrice = 0;
            foreach (var item in sepet.ProductList)
            {
                sepet.TotalSalePrice += item.TotalPrice;
            }

            Session["Sepet"] = sepet;
            return RedirectToAction("Index");
        }

        public JsonResult ChangeProductSalePrice(ChangePriceVM model)
        {
            if (model.Price != 0)
            {
                var sepet = (CartVM)Session["Sepet"];
                var selectedItem = sepet.ProductList.FirstOrDefault(x => x.ID == model.ID);

                sepet.TotalSalePrice -= selectedItem.TotalPrice;
                selectedItem.TotalPrice = model.Price;
                selectedItem.UnitPrice = Math.Round((model.Price / (selectedItem.KDV + 1)) / selectedItem.SaleCount, 4);
                selectedItem.UnitSalePrice = selectedItem.UnitPrice * selectedItem.SaleCount;
                sepet.TotalSalePrice += selectedItem.TotalPrice;
                selectedItem.KdvPrice = Math.Round(selectedItem.TotalPrice - selectedItem.UnitSalePrice, 4);
                return Json("succ");
            }
            else
            {
                return Json("fail");
            }
        }

        [HttpPost]
        public ActionResult AddSale(SaleVM model)
        {
            var sepet = (CartVM)Session["Sepet"];
            if (sepet != null)
            {
                int userId = UserID();
                model.InvoiceDate = model.InvoiceDate.Year == 0001 ? DateTime.Now : model.InvoiceDate;
                Sale sale = new Sale()
                {
                    PaymentType = model.PaymentType,
                    UserID = userId,
                    CustomerID = model.CustomerID,
                    InvoiceDate = model.InvoiceDate,
                    ExpiryDate = model.ExpiryDate.Year == 0001 ? DateTime.Now : model.ExpiryDate,
                    TotalPrice = sepet.TotalSalePrice,
                    IsInvoiced = model.Invoice == 1 ? true : false
                };
                var list = new List<SaleDetails>();
                foreach (var item in sepet.ProductList)
                {
                    SaleDetails detail = new SaleDetails()
                    {
                        ProductID = item.ID,
                        KdvPrice = Math.Round(item.KdvPrice, 2),
                        Quantity = item.SaleCount,
                        Price = Math.Round(item.UnitSalePrice, 2),
                        AddDate = DateTime.Now,
                        IsPhone = item.CategoryID == 6 || item.CategoryID == 7 ? true : false,
                        UnitSalePrice = item.UnitPrice,
                    };
                    list.Add(detail);
                }
                sale.SaleDetails = list;
                rpsale.Add(sale);
                SetPaymentInfoforAdding(sale.SaleDetails, sepet.ProductList);

                if (model.PaymentType == "Vadeli")
                {
                    CustomerExpiry expiry = new CustomerExpiry()
                    {
                        ExpiryDate = model.ExpiryDate,
                        CustomerID = model.CustomerID,
                        PaidPrice = model.PaidExpiryValue,
                        SaleID = sale.ID,
                        SaleTotalPrice = sepet.TotalSalePrice
                    };
                    ExpiryService.SetCustomerExpiry(expiry);
                    rpcustomerexpiry.Add(expiry);
                    if (model.PaidExpiryValue != 0)
                    {
                        var expiryPayment = new ExpiryPayment
                        {
                            AdminUserID = UserID(),
                            PersonID = model.CustomerID,
                            Price = model.PaidExpiryValue
                        };
                        rpexpirypayment.Add(expiryPayment);
                    }
                }
                if (model.Invoice == 1)
                {
                    return RedirectToAction("SetInvoice", model);
                }
                Session.Remove("Sepet");
            }
            return RedirectToAction("Index");
        }

        public ActionResult SetInvoice(SaleVM saleVM)
        {
            var sepet = (CartVM)Session["Sepet"];
            var customer = rpcustomer.Find(saleVM.CustomerID);
            InvoiceVM model = new InvoiceVM();
            model.CustomerName = customer.Name;
            model.Address = customer.Address;
            model.City = rpcity.Find(customer.CityID).Name;
            model.Region = rpregion.Find(customer.RegionID).Name;
            model.TaxOffice = customer.IsPerson ? "" : customer.TaxOffice;
            model.TaxNumber = customer.IsPerson ? "T.C. " + customer.TCNo : customer.TaxNumber;
            model.ProductList = sepet.ProductList;
            model.TotalSalePrice = sepet.TotalSalePrice;
            model.PriceString = saleVM.PriceString;

            model.DateOfArrangement = String.Format("{0:d/M/yyyy}", saleVM.InvoiceDate);
            model.HourOfArrangement = String.Format("{0:HH:mm}", saleVM.InvoiceDate);

            decimal kdv = 0;
            model.KDVList = new List<decimal>();
            model.KDVStringList = new List<string>();
            model.KDVList.Add(model.TotalUnitPrice);
            model.KDVStringList.Add("TOPLAM");
            foreach (var item in model.ProductList)
            {
                if (item.KDV != kdv)
                {
                    model.KDVList.Add(Math.Round(model.ProductList.Where(x => x.KDV == item.KDV).Sum(x => x.KdvPrice), 2));
                    model.KDVStringList.Add("KDV %" + Convert.ToInt32(item.KDV * 100));
                }
                kdv = item.KDV;
            }
            model.KDVList.Add(model.TotalSalePrice);
            model.KDVStringList.Add("G.TOPLAM");
            Session.Remove("Sepet");
            return View(model);
        }

        [HttpPost]
        public ActionResult AddServiceSale(ServiceSaleVM model)
        {
            if (ModelState.IsValid)
            {
                int userId = UserID();
                ServiceSale entity = new ServiceSale()
                {
                    PaymentType = model.PaymentType,
                    Price = model.Price,
                    UserID = userId,
                    Note = model.Note
                };
                rpservicesale.Add(entity);
            }
            ViewBag.IslemDurum = EnumIslemDurum.Basarili;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult AddCustomer(CustomerVM model)
        {
            GetAllCitiesforAdding();
            if (ModelState.IsValid)
            {
                bool tcKontrol = false;
                model.TCNo = model.TCNo == null ? "" : model.TCNo;
                if (model.TCNo.Length == 11)
                    tcKontrol = TCNoKontrolu(model.TCNo);

                if (tcKontrol || model.TCNo == "")
                {
                    bool isCustomerExist = false;
                    isCustomerExist = model.TaxNumber != null ? rpcustomer.Any(x => x.TaxNumber == model.TaxNumber) : rpcustomer.Any(x => x.TCNo == model.TCNo);
                    if (isCustomerExist)
                    {
                        return Fail(FormMessages.NameExist);
                    }
                    else
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
                        return Success(FormMessages.Success);
                    }
                }
                else
                {
                    return Fail(FormMessages.TCNo);
                }
            }
            else
            {
                return Fail(FormMessages.ValidationError);
            }
        }

        private void GetCustomers()
        {
            ViewData["customer"] = rpcustomer.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name + " Tel:" + x.Phone,
                Value = x.ID.ToString()
            }).ToList();
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

        private static bool TCNoKontrolu(string TCNo)
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

        private List<int> SetPaymentInfo(int productId)
        {
            List<int> list = new List<int>();
            var paymentInfo = rppaymentinfo.FirstOrDefault(x => x.ProductID == productId && x.Count != 0);
            paymentInfo.Count -= 1;
            rppaymentinfo.SaveChanges();
            list.Add(paymentInfo.ID);
            return list;
        }

        private void SetPaymentInfoforAdding(List<SaleDetails> details, List<ProductVM> products)
        {
            for (int i = 0; i < details.Count; i++)
            {
                if (details[i].IsPhone)
                {
                    var imeiList = products[i].ImeiList;
                    foreach (var item in imeiList)
                    {
                        List<PaymentInfoDto> list = new List<PaymentInfoDto>();
                        PaymentInfoDto dto = new PaymentInfoDto();
                        var imei = rpimei.Find(item);
                        imei.IsSold = true;
                        var paymentInfo = rppaymentinfo.Find(imei.PaymentInfoID);
                        paymentInfo.Count--;
                        dto.PaymentInfoID = imei.PaymentInfoID;
                        dto.Quantity = 1;
                        dto.IMEI = imei.IMEINumber;
                        list.Add(dto);
                        AddSaleDetailInfo(details[i], list);
                    }
                }
                else
                {
                    List<PaymentInfoDto> list = new List<PaymentInfoDto>();
                    int infoCount = 0;
                    int fark = 0;
                    int quantity = details[i].Quantity;
                    while (fark >= infoCount)
                    {
                        PaymentInfoDto dto = new PaymentInfoDto();
                        var paymentInfo = rppaymentinfo
                        .FirstOrDefault(x => x.ProductID == details[i].ProductID && x.Count != 0);
                        //if (selectedItem.PaymentInfoIDs.Contains(paymentInfo.ID))
                        //{
                        //    paymentInfo.Count = paymentInfo.Count + selectedItem.SaleCount;
                        //}
                        infoCount = paymentInfo.Count;
                        if (infoCount >= quantity)
                        {
                            paymentInfo.Count -= quantity;
                            rppaymentinfo.SaveChanges();
                            dto.PaymentInfoID = paymentInfo.ID;
                            dto.Quantity = quantity;
                            list = GetPaymentInfoIdList(list, dto);
                        }
                        else
                        {
                            fark = quantity - infoCount;
                            int islemSayisi = quantity - fark;
                            quantity -= islemSayisi;
                            paymentInfo.Count -= islemSayisi;
                            infoCount = paymentInfo.Count;
                            rppaymentinfo.SaveChanges();
                            dto.PaymentInfoID = paymentInfo.ID;
                            dto.Quantity = islemSayisi;
                            list = GetPaymentInfoIdList(list, dto);
                        }
                    }
                    AddSaleDetailInfo(details[i], list);
                }
            }
        }

        private void AddSaleDetailInfo(SaleDetails detail, List<PaymentInfoDto> list)
        {
            List<SaleDetailInfo> infoList = new List<SaleDetailInfo>();
            foreach (var item in list)
            {
                var entity = new SaleDetailInfo
                {
                    PaymentInfoID = item.PaymentInfoID,
                    SaleDetailID = detail.ID,
                    AddDate = DateTime.Now,
                    Quantity = item.Quantity,
                    IMEI = item.IMEI
                };
                infoList.Add(entity);
            }
            rpsaledetailinfo.AddRange(infoList);
            rpimei.SaveChanges();
            rppaymentinfo.SaveChanges();
        }

        private List<PaymentInfoDto> GetPaymentInfoIdList(List<PaymentInfoDto> list, PaymentInfoDto dto)
        {
            bool varMi = list.Count == 0 ? true : false;
            foreach (var item in list)
            {
                if (item.PaymentInfoID != dto.PaymentInfoID)
                {
                    varMi = true;
                }
            }
            if (varMi)
            {
                list.Add(dto);
            }
            return list;
        }

        public JsonResult GetPhoneProducts()
        {
            var sepet = (CartVM)Session["Sepet"];
            List<ProductVM> list = new List<ProductVM>();
            if (sepet != null)
            {
                foreach (var item in sepet.ProductList)
                {
                    if ((item.CategoryID == 6 || item.CategoryID == 7) && item.SaleCount > item.ImeiList.Count)
                    {
                        list.Add(item);
                    }
                }
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIMEIbyProductID(int id)
        {
            var sepet = (CartVM)Session["Sepet"];
            var selectedItem = sepet.ProductList.FirstOrDefault(x => x.ID == id);

            ImeiOprDto dto = new ImeiOprDto();
            dto.ImeiList = rpimei.GetListWithQuery(x => x.ProductID == selectedItem.ID && x.IsSold == false).Select(x => new ImeiDto
            {
                ID = x.ID,
                IMEI = x.IMEINumber
            }).ToList();

            dto.Count = selectedItem.SaleCount - selectedItem.ImeiList.Count;
            return Json(dto, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddImei(FormCollection collection)
        {
            int count = Convert.ToInt32(collection["count"]);
            List<int> imeiFormList = new List<int>();

            for (int i = 0; i < count; i++)
            {
                int id = Convert.ToInt32(collection["imei-number-select_" + i]);
                if (!imeiFormList.Any(x => x == id))
                {
                    imeiFormList.Add(id);
                }
                else
                {
                    return Fail("Lütfen farklı IMEI numalaraları seçin.");
                }
            }

            int productId = Convert.ToInt32(collection["product"]);
            var sepet = (CartVM)Session["Sepet"];
            var selectedItem = sepet.ProductList.FirstOrDefault(x => x.ID == productId);

            foreach (var item in imeiFormList)
            {
                if (!selectedItem.ImeiList.Any(x => x == item))
                {
                    selectedItem.ImeiList.Add(item);
                }
                else
                {
                    return Fail("Lütfen farklı IMEI numalaraları seçin.");
                }
            }
            sepet.ImeiCount = AddedImeiCount(sepet.ProductList);
            Session["Sepet"] = sepet;
            return Success(FormMessages.Success + " Lütfen Bekleyin...");
        }

        private int AddedImeiCount(List<ProductVM> productList)
        {
            int count = 0;
            foreach (var item in productList)
            {
                count += item.ImeiList.Count;
            }
            return count;
        }

    }
}