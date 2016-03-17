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

        public ActionResult Index()
        {
            Database db = new Database();

            // Get list of distinct car makers
            var carMakes = cmRepo.GetCarMakes();

            List<SelectListItem> carMakers = new List<SelectListItem>();
            carMakers.Add(new SelectListItem { Text = "Car Brand" });

            foreach (var carMake in carMakes)
            {
                string text = carMake.GetType().GetProperty("CarMake").GetValue(carMake).ToString();
                carMakers.Add(new SelectListItem { Value = text, Text = text });
            }

            // Get list of distinct car models
            SqlDataReader sdrCarModels = db.GetData("select distinct upper(CarModel) CarModel from CarMMY where Active = 1 order by 1");

            List<SelectListItem> carModels = new List<SelectListItem>();
            carModels.Add(new SelectListItem { Text = "Car Model" });

            while (sdrCarModels.Read())
            {
                carModels.Add(new SelectListItem { Value = sdrCarModels["CarModel"].ToString(), Text = sdrCarModels["CarModel"].ToString() });
            }

            sdrCarModels.Close();

            db.Close();

            // Empty list of car years
            List<SelectListItem> carYears = new List<SelectListItem>();
            carYears.Add(new SelectListItem { Text = "Car Year" });

            var requestModel = new Request();
            requestModel.DDListCarMakers = carMakers;
            requestModel.DDListCarModels = carModels;
            requestModel.DDListCarYears = carYears;

            return View(requestModel);
            //return View();
        }

        [HttpPost]
        public ActionResult AddRequest(Request request)
        {
            if (ModelState.IsValid)
            {
                // Save files on server.
                string uploadDirImages = Server.MapPath(WebConfigurationManager.AppSettings["uploadPicsDirectory"].ToString());
                List<string> uploadedNewNameImages = new List<string>();

                for (int iImages = 1; iImages <= 4; iImages++)
                {
                    HttpPostedFileBase uploadedFile = Request.Files["txtUFile" + iImages];
                    if (uploadedFile != null && uploadedFile.ContentLength > 0)
                    {
                        int pos = uploadedFile.FileName.LastIndexOf(".");
                        string fileExtension = uploadedFile.FileName.Substring(pos + 1);
                        string fileNewImageName = String.Concat(Guid.NewGuid().ToString(), ".", fileExtension);

                        uploadedFile.SaveAs(String.Concat(uploadDirImages, fileNewImageName));
                        uploadedNewNameImages.Add(fileNewImageName);
                    }
                }

                





                Database db = new Database();

                // Get selected CarMMY id
                object carMMYId = db.ExecuteScalar("select CarMMYId from CarMMY where CarModel = @1 and CarYear = @2 and Active =1",
                    new object[] { request.SelectedCarModelId, request.SelectedCarYearId });

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
                c.CarMMYId = (int)carMMYId;
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


                db.Close();

                // Send email to client
                Email email = new Email(WebConfigurationManager.AppSettings["EmailSmtpHost"].ToString());

                // Get email default config parameters
                email.From = WebConfigurationManager.AppSettings["EmailFrom"].ToString();
                email.Port = Convert.ToInt32(WebConfigurationManager.AppSettings["EmailPort"].ToString());
                email.NetworkCredentialUser = WebConfigurationManager.AppSettings["EmailNetworkCredentialUser"].ToString();
                email.NetworkCredentialPassword = WebConfigurationManager.AppSettings["EmailNetworkCredentialPassword"].ToString();

                email.To = request.Email;
                email.Subject = "We've received your request! - Fyxme.com";

                // Get HTML template for Lead Request Received confirmation
                StreamReader sr = new StreamReader(Server.MapPath("~/Content/MailTemplates/MailSendLeadConfirmRequestReceived.html"));
                string htmlMailTemplate = sr.ReadToEnd();

                /*email.Body = String.Format(htmlMailTemplate,
                    Server.MapPath("~/Content/Images/fyxmy_logo_poster_.png"), request.FirstName, request.LastName, caseId.ToString(), request.Email, request.PhoneNumber, request.DamageDescription);*/

                //htmlMailTemplate = htmlMailTemplate.Replace("{0}", Server.MapPath("~/Content/Images/fyxmy_logo_poster_.png"));
                htmlMailTemplate = htmlMailTemplate.Replace("{0}", "http://www.fyxme.com/images/fyxmy_logo_poster_.png");
                htmlMailTemplate = htmlMailTemplate.Replace("{1}", request.FirstName.ToUpper());
                htmlMailTemplate = htmlMailTemplate.Replace("{2}", request.LastName.ToUpper());
                htmlMailTemplate = htmlMailTemplate.Replace("{3}", caseId.ToString());
                htmlMailTemplate = htmlMailTemplate.Replace("{4}", request.Email);
                htmlMailTemplate = htmlMailTemplate.Replace("{5}", request.PhoneNumber);
                htmlMailTemplate = htmlMailTemplate.Replace("{6}", request.DamageDescription);

                email.Body = htmlMailTemplate;
                email.Send();

                // Send email to fyxme admin
                // Get HTML template for Admin Request Received confirmation
                sr = new StreamReader(Server.MapPath("~/Content/MailTemplates/MailSendAdminConfirmRequestReceived.html"));
                htmlMailTemplate = sr.ReadToEnd();

                email.To = WebConfigurationManager.AppSettings["EmailAdmin"].ToString();
                email.Subject = "You've received a new request! - Fyxme.com";

                htmlMailTemplate = htmlMailTemplate.Replace("{0}", "http://www.fyxme.com/images/fyxmy_logo_poster_.png");
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
                foreach (string uploadedImage in uploadedNewNameImages)
                {
                    pics += "<a href='http://" + Request.Url.Authority + "/Content/Upload/Pictures/" + uploadedImage + "'>http://" + Request.Url.Authority + "/Content/Upload/Pictures/" + uploadedImage + " </a><br/>";
                }

                htmlMailTemplate = htmlMailTemplate.Replace("{11}", pics);

                email.Body = htmlMailTemplate;
                email.Send();

                // Redirect to confirmation view.
                return RedirectToAction("RequestSent");
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
            Database db = new Database();

            // Get list of car models filtered by car brand selected
            SqlDataReader sdrCarModels;

            if (Request["value"].ToString() != "")
            {
                sdrCarModels = db.GetData("select distinct upper(CarModel) CarModel from CarMMY where CarMake = '" + Request["value"].ToString() + "' and Active = 1 order by 1");
            }
            else
            {
                sdrCarModels = db.GetData("select distinct upper(CarModel) CarModel from CarMMY where Active = 1 order by 1");
            }

            // Add default option
            List<SelectListItem> carModels = new List<SelectListItem>();
            carModels.Add(new SelectListItem { Text = "Car Model" });

            // Add list options
            while (sdrCarModels.Read())
            {
                carModels.Add(new SelectListItem { Value = sdrCarModels["CarModel"].ToString(), Text = sdrCarModels["CarModel"].ToString() });
            }
            
            sdrCarModels.Close();
            db.Close();

            return this.Json(carModels);
        }

        [HttpPost]
        public JsonResult UpdateCarYears()
        {
            Database db = new Database();

            // Get list of car models filtered by car brand selected
            SqlDataReader sdrCarYears = db.GetData("select distinct CarYear from CarMMY where CarModel = '" + Request["value"].ToString() + "' order by 1");

            // Add default option
            List<SelectListItem> carYears = new List<SelectListItem>();
            carYears.Add(new SelectListItem { Text = "Car Year" });

            // Add list options
            while (sdrCarYears.Read())
            {
                carYears.Add(new SelectListItem { Value = sdrCarYears["CarYear"].ToString(), Text = sdrCarYears["CarYear"].ToString() });
            }

            sdrCarYears.Close();
            db.Close();

            return this.Json(carYears);
        }
    }
}