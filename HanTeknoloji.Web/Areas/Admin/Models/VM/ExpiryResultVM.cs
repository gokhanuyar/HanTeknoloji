using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class ExpiryResultVM
    {
        public string SaleDate { get; set; }

        public string ExpiryDate { get; set; }

        public decimal SalePrice { get; set; }

        public decimal PaidPrice { get; set; }

        public decimal ExpiryValue { get; set; }
    }
}