using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class TradeMarkVM : BaseVM
    {
        [Display(Name = "Marka Adı"), Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string Name { get; set; }

        [Display(Name = "Barkod Değeri"), Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string BarcodeValue { get; set; }
    }
}