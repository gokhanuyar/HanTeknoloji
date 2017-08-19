using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class ServiceSaleVM : BaseVM
    {
        public string CustomerName { get; set; }

        public string ProductModel { get; set; }

        public int EmployeeID { get; set; }

        public string IMEINumber { get; set; }

        public string PaymentType { get; set; }

        public decimal Price { get; set; }

        public string Note { get; set; }
    }
}