using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class TechnicalServiceCost : BaseEntity
    {
        public decimal Cost { get; set; }

        public DateTime CostDate { get; set; }
    }
}
