﻿using HanTeknoloji.Web.Areas.Admin.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HanTeknoloji.Web.Areas.Admin.Controllers
{
    [RolControl(EnumRoles.Manager)]
    public class AdminHomeController : AdminBaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}