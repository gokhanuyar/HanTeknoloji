using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class TechnicalServiceCostVM : BaseVM
    {
        [Required(ErrorMessage = "Bu alan boş geçilemez."), Display(Name = "Tarih")]
        public DateTime CostDate { get; set; }

        [Required(ErrorMessage = "Bu alan boş geçilemez."), Display(Name = "Miktar (TL)"), Range(typeof(Decimal), "1", "50000", ErrorMessage = "{0} {1} ve {2} arasında bir değer olmalı.")]
        public decimal Cost { get; set; }

        [Display(Name = "Tarih")]
        public string CostDateString { get; set; }
    }
}