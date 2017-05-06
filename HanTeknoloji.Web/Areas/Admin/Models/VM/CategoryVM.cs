using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class CategoryVM : BaseVM
    {
        public string CategoryName { get; set; }

        public string BarcodeValue { get; set; }
    }
}