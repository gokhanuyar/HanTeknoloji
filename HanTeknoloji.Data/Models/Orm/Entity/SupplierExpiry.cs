using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class SupplierExpiry : BaseEntity
    {
        public int SupplierID { get; set; }

        public int ProductID { get; set; }

        public int ProductCount { get; set; }

        public decimal PaidPrice { get; set; }

        public decimal TotalBuyingPrice { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}
