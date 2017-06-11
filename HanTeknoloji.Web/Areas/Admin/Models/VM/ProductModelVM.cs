using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class ProductModelVM : BaseVM
    {
        [Display(Name = "Model"), Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string Name { get; set; }

        [Display(Name = "Marka"), Required(ErrorMessage = "Lütfen bir marka seçiniz.")]
        public int TradeMarkID { get; set; }

        [Display(Name = "Barkod Değeri"), Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string BarcodeValue { get; set; }
    }
}