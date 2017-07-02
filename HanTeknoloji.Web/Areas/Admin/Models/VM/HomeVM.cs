using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class HomeVM
    {
        public int SaleCount { get; set; }

        public decimal SaleTotal { get; set; }

        public int ServiceSaleCount { get; set; }

        public decimal ServiceSaleTotal { get; set; }

        public int FewProductCount { get; set; }
    }
}