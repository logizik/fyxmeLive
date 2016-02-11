using System;
using System.IO;
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
                // Save files on server.
                string uploadDirImages = Server.MapPath(WebConfigurationManager.AppSettings["uploadPicsDirectory"].ToString());
                for (int iImages = 1; iImages <= 4; iImages++)
                {
                    HttpPostedFileBase uploadedFile = Request.Files["txtUFile" + iImages];
                    if (uploadedFile != null && uploadedFile.ContentLength > 0)
                    {
                        int pos = uploadedFile.FileName.LastIndexOf(".");
                        string fileExtension = uploadedFile.FileName.Substring(pos + 1);
                        string fileNameTemp = String.Concat(Guid.NewGuid().ToString(), ".", fileExtension);

                        uploadedFile.SaveAs(String.Concat(uploadDirImages, fileNameTemp));
                    }
                }

                Database db = new Database();

                // Add client's request.
                /*object requestId = db.ExecuteScalar("insert into Request (LeadFirstName, LeadLastName, LeadEmail, LeadPhoneNumber, LeadZipCode, Origin, CreatedBy) output inserted.RequestId values (@1, @2, @3, @4, @5, @6, 0)",
                    new object[] { request.FirstName,
                    request.LastName,
                    request.Email,
                    request.PhoneNumber.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", ""),
                    request.ZipCode,
                    "WEB" });*/

                int requestId = db.ExecuteSP("usp_InsertLead",
                    new string[] { "FirstName", "LastName", "Email", "PhoneNumber", "ZipCode", "LeadStatus", "Origin", "CreatedBy" },
                    new object[] { request.FirstName,
                    request.LastName,
                    request.Email,
                    request.PhoneNumber.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", ""),
                    request.ZipCode,
                    0,
                    "WEB",
                    0}, "LeadId");

                

                // Add case.
                /*.Execute("insert into [Case] (RequestId, CarRank, CarMMYId, CarDesc, CaseStatus, ManagerId, CreatedBy) values (@1, 1, 1, @2, 1, 0, 0)",
                    new object[] { Convert.ToInt32(requestId),
                    request.DamageDescription });*/

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