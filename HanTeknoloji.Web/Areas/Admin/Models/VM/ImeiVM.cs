using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class ImeiVM
    {
        [Required(ErrorMessage = "Bu alan boş geçilemez.")]
        public string IMEINumber { get; set; }
    }
}