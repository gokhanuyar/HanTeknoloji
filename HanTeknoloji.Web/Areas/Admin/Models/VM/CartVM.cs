using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class CartVM
    {
        public decimal TotalSalePrice { get; set; }

        public decimal TotalSalePriceKdv { get; set; }

        public List<ProductVM> ProductList { get; set; }
        
        public int PhoneCount { get; set; }
        public int ImeiCount { get; set; }
    }
}