﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMY.WEB.UI.Controllers.VIP
{
    public class ListController:Controller
    {
        public ActionResult Index()
        {
            Response.Write("VipUserList");
            return View();
        }
    }
}