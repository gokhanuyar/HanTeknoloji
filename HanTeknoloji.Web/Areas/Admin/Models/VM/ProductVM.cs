using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class ProductVM : BaseVM
    {
        [Display(Name = "Seri Numarası")]
        public string SerialNumber { get; set; }

        [Display(Name = "Kategori"), Required(ErrorMessage = "Lütfen kategori seçiniz.")]
        public int CategoryID { get; set; }

        [Display(Name = "Marka"), Required(ErrorMessage = "Lütfen marka seçiniz.")]
        public int TradeMarkID { get; set; }

        [Display(Name = "Model")]
        public int ProductModelID { get; set; }

        [Display(Name = "Kategori")]
        public string Category { get; set; }

        [Display(Name = "Marka")]
        public string TradeMark { get; set; }

        [Display(Name = "Model")]
        public string ProductModel { get; set; }

        [Display(Name = "Tedarikçi"), Required(ErrorMessage = "Tedarikçi alanı boş geçilemez.")]
        public int SupplierID { get; set; }
        public string Supplier { get; set; }

        [Display(Name = "Renk")]
        public int ColorID { get; set; }

        [Display(Name = "Renk")]
        public string Color { get; set; }

        [Display(Name = "Alış Fiyatı"), Required(ErrorMessage = "Alış fiyatı boş geçilemez."), Range(typeof(Decimal), "1", "50000", ErrorMessage = "{0} {1} ve {2} arasında bir değer olmalı.")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Satış Fiyatı"), Required(ErrorMessage = "Satış fiyatı boş geçilemez."), Range(typeof(Decimal), "1", "50000", ErrorMessage = "{0} {1} ve {2} arasında bir değer olmalı.")]
        public decimal UnitSalePrice { get; set; }

        [Display(Name = "Adet"), Required(ErrorMessage = "Lütfen ürün adeti giriniz."), Range(typeof(Int32), "1", "1000", ErrorMessage = "{0} {1} ve {2} arasında bir değer olmalı.")]
        public int Count { get; set; }

        [Display(Name = "Ödeme Şekli"), Required(ErrorMessage = "Ödeme şekli alanı boş geçilemez.")]
        public string Payment { get; set; }

        [Display(Name = "KDV"), Required(ErrorMessage = "KDV alanı boş geçilemez")]
        public decimal KDV { get; set; }

        [Display(Name = "IMEI")]
        public string IMEI { get; set; }

        public int SaleCount { get; set; }
        public decimal UnitBuyPrice { get; set; }

        public int PaymentInfoID { get; set; }
        public string BankName { get; set; }
        public string BankCartName { get; set; }
        public string CartNumber { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string ExpiryDate { get; set; }
        public decimal PaidPrice { get; set; }
        public string CheckNumber { get; set; }

        private decimal totalPrice;
        public decimal TotalPrice
        {
            get
            {
                return totalPrice != 0 ? totalPrice : Math.Round(UnitSalePrice + KdvPrice, 2);
            }
            set
            {
                this.totalPrice = value;
            }
        }

        public string FullProductName
        {
            get
            {
                return TradeMark + " " + ProductModel;
            }
        }

        public decimal KdvPrice { get; set; }
    }
}