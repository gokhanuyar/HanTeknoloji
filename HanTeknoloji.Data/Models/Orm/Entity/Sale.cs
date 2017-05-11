using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class Sale : BaseEntity
    {
        public int ProductID { get; set; }

        public decimal Price { get; set; }

        public string PaymentType { get; set; }

        public int Quantity { get; set; }

        public int UserID { get; set; }

        public int CustomerID { get; set; }
    }
}
