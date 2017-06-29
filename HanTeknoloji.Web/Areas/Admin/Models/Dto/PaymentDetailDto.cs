using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.Dto
{
    public class PaymentDetailDto
    {
        public string Payment { get; set; }

        public string BankName { get; set; }

        public string BankCardName { get; set; }

        public string CardNumber { get; set; }

        public string ExpiryDate { get; set; }

        public string CheckNumber { get; set; }
    }
}