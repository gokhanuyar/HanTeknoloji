using HanTeknoloji.Web.Areas.Admin.Models.Dto;
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

        public int SupplierExpiryCount { get; set; }
        public decimal SupplierExpiryValue { get; set; }

        public int CustomerExpiryCount { get; set; }
        public decimal CustomerExpiryValue { get; set; }

        public int SupplierExpiryPastedCount { get; set; }
        public decimal SupplierExpiryPastedValue { get; set; }

        public int CustomerExpiryPastedCount { get; set; }
        public decimal CustomerExpiryPastedValue { get; set; }
    }
}