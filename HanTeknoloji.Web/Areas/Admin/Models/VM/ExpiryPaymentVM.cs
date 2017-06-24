using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class ExpiryPaymentVM
    {
        public string AdminUserName { get; set; }

        public string Date { get; set; }

        public string Hour { get; set; }

        public decimal Price { get; set; }
    }
}