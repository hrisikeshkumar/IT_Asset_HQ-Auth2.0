using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace IT_Hardware.Controllers
{
    public class Log_InController : Controller
    {
        // GET: Log_In
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Log_In()
        {
            return View("Log_In");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Sign_In()
        {
            return RedirectToAction("Admin_Dashboard", "Admin_Dashboard", new { area = "Admin" });
        }

        [HttpGet]
        public ActionResult Log_Out()
        {
            //FormsAuthentication.SignOut();
            return RedirectToAction("Log_In");

        }
    }
}


