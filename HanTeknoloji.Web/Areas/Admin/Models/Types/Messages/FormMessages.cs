using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanTeknoloji.Web.Areas.Admin.Models.Types.Messages
{
    public class FormMessages
    {
        public static string Success = "İşlem başarıyla gerçekleşti";
        public static string ValidationError = "Lütfen gerekli alanları doldurunuz";
        public static string UserLoginError = "Kullanıcı adı veya şifre yanlış";
        public static string AddAdminError = "Bu emaille bir admin mevcut. Lütfen başka bir email adresi giriniz";
        public static string BarcodeError = "Girilen barkod numarası mevcut. Lütfen başka bir değer giriniz";
        public static string ProductQuantity = "Girilen ürün stokta mevcut değildir.";
        public static string NoProduct = "Girilen ürün stokta mevcut değildir.";
    }
}