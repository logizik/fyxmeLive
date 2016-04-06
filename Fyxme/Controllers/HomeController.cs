using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.IO;
using Fyxme.Models;
using fyxme.Data.Model;
using fyxme.Data.Entities;

namespace Fyxme.Controllers
{
    public class HomeController : Controller
    {
        private cmRepository cmRepo = new cmRepository();

        public static List<CarMMY> carsMMYY = new List<CarMMY>();

        public ActionResult Index(string showConfirmBox)
        {
            // Get list of distinct car makers
            var carMakes = cmRepo.GetCarMakes();

            List<SelectListItem> cboMakes = new List<SelectListItem>();
            cboMakes.Add(new SelectListItem { Text = "Car Brand" });
            cboMakes.Add(new SelectListItem { Text = "--I don't know--" });

            foreach (var carMake in carMakes)
            {
                string text = carMake.GetType().GetProperty("CarMake").GetValue(carMake).ToString().ToUpper();
                cboMakes.Add(new SelectListItem { Value = text, Text = text });
            }

            // Get list of distinct car models
            List<SelectListItem> cboModels = new List<SelectListItem>();
            cboModels.Add(new SelectListItem { Text = "Car Model" });

            // Empty list of car years
            List<SelectListItem> cboYears = new List<SelectListItem>();
            cboYears.Add(new SelectListItem { Text = "Car Year" });

            var requestModel = new RequestViewModel();
            requestModel.DDListCarMakers = cboMakes;
            requestModel.DDListCarModels = cboModels;
            requestModel.DDListCarYears = cboYears;

            if (!String.IsNullOrEmpty(showConfirmBox))
            {
                ViewBag.ConfirmBox = showConfirmBox;
            }

            return View(requestModel);
        }

        [HttpPost]
        public ActionResult AddRequest(RequestViewModel request)
        {
            if (ModelState.IsValid)
            {
                // Save files on server.
                string uploadDirImages = Server.MapPath(WebConfigurationManager.AppSettings["uploadPicsDirectory"].ToString());
                List<string> uploadedNewNameImages = new List<string>();

                for (int iImages = 1; iImages <= 4; iImages++)
                {
                    HttpPostedFileBase uploadedFile = Request.Files["txtUFile" + iImages];
                    if (uploadedFile != null && uploadedFile.ContentLength > 0 && Request["txtUploadPic" + iImages].Length > 0)
                    {
                        int pos = uploadedFile.FileName.LastIndexOf(".");
                        string fileExtension = uploadedFile.FileName.Substring(pos + 1);
                        string fileNewImageName = String.Concat(Guid.NewGuid().ToString(), ".", fileExtension);

                        uploadedFile.SaveAs(String.Concat(uploadDirImages, fileNewImageName));
                        uploadedNewNameImages.Add(fileNewImageName);
                    }
                }

                // Get selected CarMMY id
                int carMMYId = 0;
                if (!String.IsNullOrEmpty(request.SelectedCarModelId) && !String.IsNullOrEmpty(request.SelectedCarYearId)) {
                    carMMYId = cmRepo.GetCarMMYId(request.SelectedCarModelId, Convert.ToInt32(request.SelectedCarYearId));
                }

                // Add lead object
                // Lead
                Lead l = new Lead();
                l.FirstName = request.FirstName;
                l.LastName = request.LastName;
                l.Email = request.Email;
                l.PhoneNumber = request.PhoneNumber;
                l.ZipCode = request.ZipCode;
                l.StatusId = (int)EStatus.Received;     // Default status
                l.Origin = "Fyxme Website";             // Default origin
                l.CreatedBy = 999;                      // Added by customer
                
                // Case
                Case c = new Case();
                if (carMMYId > 0) {
                    c.CarMMYId = (int)carMMYId;
                }
                c.CaseDesc = request.DamageDescription;
                c.StatusId = (int)EStatus.Received;
                c.SalesRepId = 0;
                c.CreatedBy = 999;                      // Added by customer

                // Case pictures
                int picRank = 1;
                List<CasePicture> cp = new List<CasePicture>();

                foreach (string uploadedImage in uploadedNewNameImages)
                {
                    CasePicture casePic = new CasePicture()
                    {
                        PictureRank = (byte)picRank,
                        PictureName = uploadedImage,
                        PictureLocation = uploadDirImages,
                        CreatedBy = 999,
                    };

                    cp.Add(casePic);
                    picRank++;
                }

                c.CasePictures = cp;
                l.Cases.Add(c);

                // Save lead object and get CaseId
                long caseId = cmRepo.AddLead(l);

                // Send emails
                SendEmailToClient(request, caseId);
                SendEmailToAdmin(request, caseId, uploadedNewNameImages);

                // Redirect to confirmation view.
                return RedirectToAction("Index", "Home", new { showConfirmBox = "Customer" });
            }

            return RedirectToAction("Index");
        }

        public ActionResult RequestSent()
        {
            //ViewBag.RequestSent = true;

            return View();
        }

        [HttpPost]
        public JsonResult UpdateCarModels()
        {
            // Get list of car models filtered by car brand selected
            System.Collections.IEnumerable carModels;

            if (Request["value"].ToString() != "")
            {
                carModels = cmRepo.GetCarModels(Request["value"].ToString());
            }
            else
            {
                carModels = cmRepo.GetCarModels();
            }

            // Add default option
            List<SelectListItem> cboModels = new List<SelectListItem>();
            cboModels.Add(new SelectListItem { Text = "Car Model" });

            // Add list options
            foreach (var carModel in carModels)
            {
                string text = carModel.GetType().GetProperty("CarModel").GetValue(carModel).ToString().ToUpper();
                cboModels.Add(new SelectListItem { Value = text, Text = text });
            }

            return this.Json(cboModels);
        }

        [HttpPost]
        public JsonResult UpdateCarYears()
        {
            // Get list of car models filtered by car brand selected
            var carYears = cmRepo.GetCarYears(Request["value"].ToString());

            // Add default option
            List<SelectListItem> cboYears = new List<SelectListItem>();
            cboYears.Add(new SelectListItem { Text = "Car Year" });

            // Add list options
            foreach (var carYear in carYears)
            {
                string text = carYear.GetType().GetProperty("CarYear").GetValue(carYear).ToString();
                cboYears.Add(new SelectListItem { Value = text, Text = text });
            }

            return this.Json(cboYears);
        }

        public void SendEmailToClient(RequestViewModel request, long caseId)
        {
            // Send email to client
            Email email = new Email(WebConfigurationManager.AppSettings["EmailSmtpHost"].ToString());
            email.From = WebConfigurationManager.AppSettings["EmailFrom"].ToString();
            email.Port = Convert.ToInt32(WebConfigurationManager.AppSettings["EmailPort"].ToString());
            email.NetworkCredentialUser = WebConfigurationManager.AppSettings["EmailNetworkCredentialUser"].ToString();
            email.NetworkCredentialPassword = WebConfigurationManager.AppSettings["EmailNetworkCredentialPassword"].ToString();
            email.To = request.Email;
            email.Subject = "We've received your request! - Fyxme.com";

            // Get HTML template for Lead Request Received confirmation
            StreamReader sr = new StreamReader(Server.MapPath("~/Content/MailTemplates/MailSendLeadConfirmRequestReceived.html"));
            string htmlMailTemplate = sr.ReadToEnd();

            //htmlMailTemplate = htmlMailTemplate.Replace("{0}", Server.MapPath("~/Content/Images/fyxmy_logo_poster_.png"));
            htmlMailTemplate = htmlMailTemplate.Replace("{0}", "http://www.fyxme.com/Content/Images/fyxmy_logo_poster_.png");
            htmlMailTemplate = htmlMailTemplate.Replace("{1}", request.FirstName.ToUpper());
            htmlMailTemplate = htmlMailTemplate.Replace("{2}", request.LastName.ToUpper());
            htmlMailTemplate = htmlMailTemplate.Replace("{3}", caseId.ToString());
            htmlMailTemplate = htmlMailTemplate.Replace("{4}", request.Email);
            htmlMailTemplate = htmlMailTemplate.Replace("{5}", request.PhoneNumber);
            htmlMailTemplate = htmlMailTemplate.Replace("{6}", request.DamageDescription);

            email.Body = htmlMailTemplate;
            email.Send();
        }

        public void SendEmailToAdmin(RequestViewModel request, long caseId, List<string> imagesList)
        {
            // Send email to fyxme admin
            Email email = new Email(WebConfigurationManager.AppSettings["EmailSmtpHost"].ToString());
            email.From = WebConfigurationManager.AppSettings["EmailFrom"].ToString();
            email.Port = Convert.ToInt32(WebConfigurationManager.AppSettings["EmailPort"].ToString());
            email.NetworkCredentialUser = WebConfigurationManager.AppSettings["EmailNetworkCredentialUser"].ToString();
            email.NetworkCredentialPassword = WebConfigurationManager.AppSettings["EmailNetworkCredentialPassword"].ToString();
            email.To = WebConfigurationManager.AppSettings["EmailAdmin"].ToString();
            email.Subject = "You've received a new request! - Fyxme.com";

            // Get HTML template for Admin Request Received confirmation
            StreamReader sr = new StreamReader(Server.MapPath("~/Content/MailTemplates/MailSendAdminConfirmRequestReceived.html"));
            string htmlMailTemplate = sr.ReadToEnd();

            htmlMailTemplate = htmlMailTemplate.Replace("{0}", "http://www.fyxme.com/Content/Images/fyxmy_logo_poster_.png");
            htmlMailTemplate = htmlMailTemplate.Replace("{1}", caseId.ToString());
            htmlMailTemplate = htmlMailTemplate.Replace("{2}", request.FirstName.ToUpper());
            htmlMailTemplate = htmlMailTemplate.Replace("{3}", request.LastName.ToUpper());
            htmlMailTemplate = htmlMailTemplate.Replace("{4}", request.Email);
            htmlMailTemplate = htmlMailTemplate.Replace("{5}", request.PhoneNumber);
            htmlMailTemplate = htmlMailTemplate.Replace("{6}", request.ZipCode);
            htmlMailTemplate = htmlMailTemplate.Replace("{7}", request.SelectedCarMakerId);
            htmlMailTemplate = htmlMailTemplate.Replace("{8}", request.SelectedCarModelId);
            htmlMailTemplate = htmlMailTemplate.Replace("{9}", request.SelectedCarYearId);
            htmlMailTemplate = htmlMailTemplate.Replace("{10}", request.DamageDescription);

            string pics = "";
            foreach (string uploadedImage in imagesList)
            {
                pics += "<a href='http://" + Request.Url.Authority + "/Content/Upload/Pictures/" + uploadedImage + "'>http://" + Request.Url.Authority + "/Content/Upload/Pictures/" + uploadedImage + " </a><br/>";
            }

            htmlMailTemplate = htmlMailTemplate.Replace("{11}", pics);

            email.Body = htmlMailTemplate;
            email.Send();
        }

        public ActionResult AboutUs()
        {
            return View("AboutUsView");
        }

        public ActionResult PrivacyPolicy()
        {
            return View("PrivacyPolicyView");
        }

        public ActionResult TermsConditions()
        {
            return View("TermsConditionsView");
        }
    }
}