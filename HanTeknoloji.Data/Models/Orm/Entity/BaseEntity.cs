using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class BaseEntity
    {
        [Key]
        public int ID { get; set; }

        public DateTime AddDate { get; set; }

        public Nullable<DateTime> UpdateDate { get; set; }

        public Nullable<DateTime> DeleteDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
