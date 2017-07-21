using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class SaleDetailInfo : BaseEntity
    {
        public int SaleDetailID { get; set; }

        public int PaymentInfoID { get; set; }

        public int Quantity { get; set; }

        public string IMEI { get; set; }
    }
}
