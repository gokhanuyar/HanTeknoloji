using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class Customer : BaseEntity
    {
        public string TCNo { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public int CityID { get; set; }

        public int RegionID { get; set; }

        public string TaxOffice { get; set; }

        public string TaxNumber { get; set; }

        public string Phone { get; set; }

        public bool IsPerson { get; set; }

        public decimal ExpiryValue { get; set; }

        public decimal PaidExpiryValue { get; set; }
    }
}
