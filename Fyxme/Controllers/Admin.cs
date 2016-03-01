using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fyxme.Models;

namespace Fyxme.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginForm login)
        {
            if (ModelState.IsValid)
            {
                // Do login
                return RedirectToAction("Case");
            }

            return View();
        }

        public ActionResult Case()
        {
            return View();
        }
    }
}