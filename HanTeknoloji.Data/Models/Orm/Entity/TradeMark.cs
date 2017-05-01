using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class TradeMark : BaseEntity
    {
        public string Name { get; set; }

        public string BarcodeValue { get; set; }
    }
}
