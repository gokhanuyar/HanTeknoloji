using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class Product : BaseEntity
    {
        public string SerialNumber { get; set; }

        public int TradeMarkID { get; set; }

        public int ProductModelID { get; set; }        

        public int SupplierID { get; set; }

        public int ColorID { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal UnitSalePrice { get; set; }

        public int Count { get; set; }

        public int CategoryID { get; set; }

        public decimal KDV { get; set; }
    }
}
