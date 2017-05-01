using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class ProductModel : BaseEntity
    {
        public string Name { get; set; }

        public int TradeMarkID { get; set; }

        public string BarcodeValue { get; set; }
    }
}
