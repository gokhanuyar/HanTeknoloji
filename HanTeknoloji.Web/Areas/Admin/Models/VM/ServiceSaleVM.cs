using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class ServiceSaleVM : BaseVM
    {
        public string PaymentType { get; set; }

        public decimal Price { get; set; }
    }
}