using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HanTeknoloji.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class AdminHomeController : AdminBaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}