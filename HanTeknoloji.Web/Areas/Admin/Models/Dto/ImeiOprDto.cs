using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.Dto
{
    public class ImeiOprDto
    {
        public List<ImeiDto> ImeiList { get; set; }

        public int Count { get; set; }
    }
}