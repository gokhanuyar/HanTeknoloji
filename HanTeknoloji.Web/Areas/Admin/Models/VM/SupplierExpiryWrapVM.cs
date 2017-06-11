using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class SupplierExpiryWrapVM
    {
        public List<SupplierExpiryVM> SupplierList { get; set; }

        public List<ExpiryResultVM> ExpiryResultList { get; set; }

        public decimal TotalExpiryValue { get; set; }

        public decimal PaidExpiryValue { get; set; }

        public string SupplierName { get; set; }

        public int SupplierID { get; set; }
    }
}