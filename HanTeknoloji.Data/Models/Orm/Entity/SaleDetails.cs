using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class SaleDetails : BaseEntity
    {
        public int SaleID { get; set; }
        [ForeignKey("SaleID")]
        public virtual Sale Sale { get; set; }

        public int ProductID { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal KdvPrice { get; set; }
    }
}
