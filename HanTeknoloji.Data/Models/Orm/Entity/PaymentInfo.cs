using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class PaymentInfo : BaseEntity
    {
        public int ProductID { get; set; }

        public string Payment { get; set; }
        public string BankName { get; set; }
        public string BankCartName { get; set; }
        public string CartNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string CheckNumber { get; set; }

        public string IMEI { get; set; }
    }
}
