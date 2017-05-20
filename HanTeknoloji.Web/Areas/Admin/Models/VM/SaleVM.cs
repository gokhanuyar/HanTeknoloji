using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class SaleVM
    {
        public decimal Price { get; set; }

        public string PaymentType { get; set; }

        public int CustomerID { get; set; }

        public int Invoice { get; set; }

        public string PriceString { get; set; }

        public string Name { get; set; }

        public DateTime InvoiceDate { get; set; }
    }
}