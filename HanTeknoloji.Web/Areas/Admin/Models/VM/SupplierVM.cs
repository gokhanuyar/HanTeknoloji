using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class SupplierVM : BaseVM
    {
        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string WebSite { get; set; }
    }
}