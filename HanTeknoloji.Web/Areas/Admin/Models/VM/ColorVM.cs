using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class ColorVM
    {
        public int ID { get; set; }

        [Display(Name = "Renk"), Required(ErrorMessage = "Renk alanı boş geçilemez.")]
        public string Name { get; set; }

        [Display(Name = "Barkod Değeri"), Required(ErrorMessage = "Barkod Değeri alanı boş geçilemez.")]
        public string BarcodeValue { get; set; }
    }
}