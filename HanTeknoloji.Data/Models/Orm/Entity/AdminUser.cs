using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanTeknoloji.Data.Models.Orm.Entity
{
    public class AdminUser : BaseEntity
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime? LastLoginDate { get; set; }
    }
}
