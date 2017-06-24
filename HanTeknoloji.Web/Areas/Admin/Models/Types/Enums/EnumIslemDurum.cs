using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.Types.Enums
{
    public enum EnumIslemDurum
    {
        Basarili = 1,
        ValidationHata = 2,
        ParolaYanlis = 3,
        AdminMevcut = 4,
        BarkodMevcut = 5,
        StokYetersiz = 6,
        UrunYok = 7,
        YanlısTCNo = 8,
        IsimMevcut = 9
    }
}