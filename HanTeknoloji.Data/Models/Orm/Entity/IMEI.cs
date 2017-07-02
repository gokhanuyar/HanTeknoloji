using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class IMEI : BaseEntity
    {
        public string IMEINumber { get; set; }

        public bool IsSold { get; set; }
    }
}
