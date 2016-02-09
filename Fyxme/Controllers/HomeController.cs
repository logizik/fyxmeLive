using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
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

                // Add client's request.
                object requestId = db.ExecuteScalar("insert into Request (LeadFirstName, LeadLastName, LeadEmail, LeadPhoneNumber, LeadZipCode, Origin, CreatedBy) output inserted.RequestId values (@1, @2, @3, @4, @5, @6, 0)",
                    new object[] { request.FirstName,
                    request.LastName,
                    request.Email,
                    request.PhoneNumber.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", ""),
                    request.ZipCode,
                    "WEB" });

                // Add case.
                db.Execute("insert into [Case] (RequestId, CarRank, CarMMYId, CarDesc, CaseStatus, ManagerId, CreatedBy) values (@1, 1, 1, @2, 1, 0, 0)",
                    new object[] { Convert.ToInt32(requestId),
                    request.DamageDescription });

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

        /*private void GetOrigin()
        {
            HttpBrowserCapabilities browserCap;
            if (((HttpCapabilitiesBase)browserCap).IsMobileDevice)
            {
                labelText = "Browser is a mobile device.";
            }
            else
            {
                labelText = "Browser is not a mobile device.";
            }

            Label1.Text = labelText;
        }*/

    }
}