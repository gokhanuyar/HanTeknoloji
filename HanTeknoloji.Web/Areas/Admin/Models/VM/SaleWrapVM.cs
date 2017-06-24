using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.VM
{
    public class SaleWrapVM
    {
        public CartVM Cart { get; set; }

        public CustomerVM Customer { get; set; }
    }
}