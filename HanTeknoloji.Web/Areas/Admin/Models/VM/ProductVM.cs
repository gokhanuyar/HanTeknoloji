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

        [Display(Name = "Renk"), Required(ErrorMessage = "Renk alanı boş geçilemez.")]
        public int ColorID { get; set; }

        [Display(Name = "Renk")]
        public string Color { get; set; }

        [Display(Name = "Birim Fiyat"), Required(ErrorMessage = "Birim fiyat boş geçilemez.")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Satış Fiyatı"), Required(ErrorMessage = "Satış fiyatı boş geçilemez.")]
        public decimal UnitSalePrice { get; set; }

        [Display(Name = "Adet"), Required(ErrorMessage = "Lütfen ürün adeti giriniz.")]
        public int Count { get; set; }

        [Display(Name = "Ödeme Şekli"), Required(ErrorMessage = "Ödeme şekli alanı boş geçilemez.")]
        public string Payment { get; set; }

        [Display(Name ="KDV"),Required(ErrorMessage ="KDV alanı boş geçilemez")]
        public decimal KDV { get; set; }

        [Display(Name = "IMEI")]
        public string IMEI { get; set; }

        public int SaleCount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}