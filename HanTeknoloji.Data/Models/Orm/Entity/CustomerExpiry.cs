using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class CustomerExpiry : BaseEntity
    {
        public int CustomerID { get; set; }

        public int SaleID { get; set; }

        public DateTime ExpiryDate { get; set; }

        public decimal ExpiryValue { get; set; }

        public decimal SaleTotalPrice { get; set; }
    }
}
