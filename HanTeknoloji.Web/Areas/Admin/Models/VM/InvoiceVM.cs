using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class InvoiceVM
    {
        public string Address { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string TaxOffice { get; set; }

        public string TaxNumber { get; set; }

        public string TCNo { get; set; }

        public List<ProductVM> ProductList { get; set; }

        public decimal TotalPrice { get; set; }
    }
}