using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class CartVM
    {
        public decimal TotalSalePrice { get; set; }

        public List<ProductVM> ProductList { get; set; }
    }
}