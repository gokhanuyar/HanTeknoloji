using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class SaleDetailsVM
    {
        public int ProductID { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal KdvPrice { get; set; }

        public DateTime AddDate { get; set; }

        public ProductVM Product { get; set; }

        public decimal UnitBuyPrice { get; set; }
        public decimal UnitSalePrice { get; set; }
    }
}