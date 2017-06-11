using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class CategoryVM : BaseVM
    {
        [Display(Name = "Kategori Adı"), Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string CategoryName { get; set; }
    }
}