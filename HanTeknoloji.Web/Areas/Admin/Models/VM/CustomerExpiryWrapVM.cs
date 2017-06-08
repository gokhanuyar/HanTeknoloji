using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class CustomerExpiryWrapVM
    {
        public List<CustomerExpiryVM> CustomerList { get; set; }

        public List<ExpiryResultVM> ExpiryResultList { get; set; }

        public decimal TotalExpiryValue { get; set; }

        public decimal PaidExpiryValue { get; set; }

        public string CustomerName { get; set; }

        public int CustomerID { get; set; }
    }
}