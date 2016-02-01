using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fyxme.Models;

namespace Fyxme.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRequest(Request request)
        {
            if (ModelState.IsValid)
            {
                Database db = new Database();

                // Add the lead.
                db.Execute("insert into Lead (FirstName, LastName, Email, PhoneNumber, ZipCode, CreatedBy) values (@1, @2, @3, @4, @5, 0)",
                    new object[] { request.FirstName,
                    request.LastName,
                    request.Email,
                    request.MobilePhone.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", ""),
                    request.ZipCode });

                db.Close();

                // Redirect to confirmation view.
                return RedirectToAction("RequestSent");
            }

            return RedirectToAction("Index");
        }

        public ActionResult RequestSent()
        {
            return View();
        }
    }
}