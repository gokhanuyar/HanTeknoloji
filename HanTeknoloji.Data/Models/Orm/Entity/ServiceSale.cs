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

        public string Note { get; set; }

        public int UserID { get; set; }

        public string CustomerName { get; set; }

        public string ProductModel { get; set; }

        public int EmployeeID { get; set; }

        public string IMEINumber { get; set; }
    }
}
