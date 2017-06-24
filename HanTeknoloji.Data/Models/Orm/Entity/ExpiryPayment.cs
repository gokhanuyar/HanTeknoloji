using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class ExpiryPayment : BaseEntity
    {
        public decimal Price { get; set; }

        public int PersonID { get; set; }

        public int AdminUserID { get; set; }
    }
}
