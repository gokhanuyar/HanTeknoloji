using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class SupplierExpiryVM : BaseVM
    {
        public int SupplierID { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public decimal Price { get; set; }
    }
}