using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Fyxme.Models;

namespace Fyxme.Controllers
{
    public class HomeController : Controller
    {
        public static List<CarMMY> carsMMYY = new List<CarMMY>();

        public ActionResult Index()
        {
            Database db = new Database();

            // Get list of distinct car makers
            SqlDataReader sdrCarMakers = db.GetData("select distinct upper(CarMake) CarMake from CarMMY where Active = 1 order by 1");

            List<SelectListItem> carMakers = new List<SelectListItem>();
            carMakers.Add(new SelectListItem { Text = "Car Brand" });

            while (sdrCarMakers.Read())
            {
                carMakers.Add(new SelectListItem { Value = sdrCarMakers["CarMake"].ToString(), Text = sdrCarMakers["CarMake"].ToString() });
            }

            sdrCarMakers.Close();

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

                // Add lead.
                int leadId = db.ExecuteSP("usp_InsertLead",
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
                int caseId = db.ExecuteSP("usp_InsertCase",
                    new string[] { "LeadId", "CarRank", "CarMMYId", "DamageDesc", "CaseStatus", "ManagerId", "CreatedBy" },
                    new object[] { leadId, 1,
                    (int)carMMYId,
                    String.IsNullOrEmpty(request.DamageDescription) ? "" : request.DamageDescription,
                    1,
                    0,
                    0}, "CaseId");

                // Add pictures
                int picRank = 1;
                foreach (string uploadedImage in uploadedNewNameImages)
                {
                    int picId = db.ExecuteSP("usp_InsertPicture",
                        new string[] { "CaseId", "PictureRank", "PictureName", "PictureLocation", "CreatedBy" },
                        new object[] { caseId, picRank,
                        uploadedImage,
                        uploadDirImages,
                        0}, "PictureId");

                    picRank++;
                }

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

                email.Body = String.Format(@"<body bgcolor='#f6f6f6'>
                    <table class='body-wrap' bgcolor='#f6f6f6'>
                    <tr>
                    <td></td>
                    <td class='container' bgcolor='#FFFFFF'>
                    <div class='content'>
                    <table>
                    <tr>
                    <td>
                    <h2>We've received your request!</h2>
                    <p>Hi {0} {1},</p>
                    <p>We appreciate you taking the time to submit your information and are committed to providing you with an easy and seamless car repair experience.</p>
                    <p>We will be following up with you shortly with your quote and car technician details.You can be assured of receiving a premium service at a very competitive price.</p>
                    <p>Should you decide to move forward with our service, a mobile car technician will come directly to you, at home or work and on your own schedule.</p>
                    <p>Below you will find the details of your request.</p>
                    <h6>Your Email:</h6>
                    <p>{2}</p>
                    <h6>Your Case Number:</h6>
                    <p>{3}</p>
                    <h6>Your Phone Number:</h6>
                    <p>{4}</p>
                    <h6>Description:</h6>
                    <p>{5}</p>
                    <p>Thanks, have a lovely day.</p>
                    </td>
                    </tr>
                    </table>
                    </div>
                    </td>
                    <td></td>
                    </tr>
                    </table>
                    </body>", request.FirstName, request.LastName, request.Email, leadId, request.PhoneNumber, request.DamageDescription);

                email.Send();

                // Redirect to confirmation view.
                return RedirectToAction("RequestSent");
            }

            return RedirectToAction("Index");
        }

        public ActionResult RequestSent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateCarModels()
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
            string ddListCarModelsHTML = "<option>Car Model</option>";

            while (sdrCarModels.Read())
            {
                // Add option
                string option = String.Concat("<option value='", sdrCarModels["CarModel"].ToString(), "'>", sdrCarModels["CarModel"].ToString(), "</option>");
                ddListCarModelsHTML += option;
            }

            sdrCarModels.Close();
            db.Close();

            return Content(String.Join("", ddListCarModelsHTML));
        }

        [HttpPost]
        public ActionResult UpdateCarYears()
        {
            Database db = new Database();

            // Get list of car models filtered by car brand selected
            SqlDataReader sdrCarYears = db.GetData("select distinct CarYear from CarMMY where CarModel = '" + Request["value"].ToString() + "' order by 1");

            // Add default option
            string ddListCarYearsHTML = "<option>Car Year</option>";

            while (sdrCarYears.Read())
            {
                // Add option
                string option = String.Concat("<option value='", sdrCarYears["CarYear"].ToString(), "'>", sdrCarYears["CarYear"].ToString(), "</option>");
                ddListCarYearsHTML += option;
            }

            sdrCarYears.Close();
            db.Close();

            return Content(String.Join("", ddListCarYearsHTML));
        }
    }
}