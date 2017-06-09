using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class Supplier : BaseEntity
    {
        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string WebSite { get; set; }

        public int CityID { get; set; }

        public int RegionID { get; set; }

        public decimal TotalExpiryValue { get; set; }

        public decimal PaidExpiryValue { get; set; }
    }
}
