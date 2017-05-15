using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class ChangePriceVM : BaseVM
    {
        [Range(0.1, 15000, ErrorMessage = " Bu alana 0.1 ile 15000 arası bir değer belirtmek zorundasınız.")]
        public decimal Price { get; set; }
    }
}