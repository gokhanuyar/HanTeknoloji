using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class ServiceSale : BaseEntity
    {
        public string PaymentType { get; set; }

        public decimal Price { get; set; }

        public int UserID { get; set; }
    }
}
