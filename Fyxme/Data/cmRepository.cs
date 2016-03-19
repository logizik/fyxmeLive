using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using fyxme.Data.Entities;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace fyxme.Data.Model
{
    // Disconnected Repository designed to server MVC controllers relating to Entities around a CASE object --> Lead, Case, Case Pictures, Case Notes...
    //
    public class cmRepository
    {

        /// <summary>
        /// GetCarMakers
        /// </summary>
        /// <returns></returns>
        public IEnumerable GetCarMakes()
        {
            using (var context = new fyxmeContext())
            {
                return context.CarMMYs.AsNoTracking()
                    .Where(c => c.Active == true)
                    .Select(c => new { c.CarMake }).Distinct()
                    .OrderBy(c => c.CarMake)
                    .ToList();
            }
        }

        /// <summary>
        /// GetCarModels
        /// </summary>
        /// <returns></returns>
        public IEnumerable GetCarModels()
        {
            using (var context = new fyxmeContext())
            {
                return context.CarMMYs.AsNoTracking()
                    .Where(c => c.Active == true)
                    .Select(c => new { c.CarModel }).Distinct()
                    .OrderBy(c => c.CarModel)
                    .ToList();
            }
        }

        /// <summary>
        /// GetCarModels
        /// </summary>
        /// <param name="carMake"></param>
        /// <returns></returns>
        public IEnumerable GetCarModels(string carMake)
        {
            using (var context = new fyxmeContext())
            {
                return context.CarMMYs.AsNoTracking()
                    .Where(c => c.Active == true && c.CarMake == carMake)
                    .Select(c => new { c.CarModel }).Distinct()
                    .OrderBy(c => c.CarModel)
                    .ToList();
            }
        }


        /// <summary>
        /// GetCarYears
        /// </summary>
        /// <param name="carModel"></param>
        /// <returns></returns>
        public IEnumerable GetCarYears(string carModel)
        {
            using (var context = new fyxmeContext())
            {
                return context.CarMMYs.AsNoTracking()
                    .Where(c => c.Active == true && c.CarModel == carModel)
                    .Select(c => new { c.CarYear }).Distinct()
                    .OrderBy(c => c.CarYear)
                    .ToList();
            }
        }

        /// <summary>
        /// GetCarMMYId
        /// </summary>
        /// <param name="carModel"></param>
        /// <param name="carYear"></param>
        /// <returns></returns>
        public int GetCarMMYId(string carModel, int carYear)
        {
            using (var context = new fyxmeContext())
            {
                return context.CarMMYs.AsNoTracking()
                    .Where(c => c.Active == true && c.CarModel == carModel && c.CarYear == carYear)
                    .Select(c => c.CarMMYId)
                    .SingleOrDefault();
            }
        }

        /// <summary>
        /// AddLead
        /// </summary>
        /// <param name="lead"></param>
        /// <returns></returns>
        public long AddLead(Lead lead)
        {
            using (var context = new fyxmeContext())
            {
                context.Leads.Add(lead);
                context.SaveChanges();

                return lead.Cases.ElementAt(0).CaseNo;
            }
        }

        public List<RecentCasesViewModel> GetRecentCasesWithLeadProjection(int noCases)
        {
            using (var context = new fyxmeContext())
            {
                return context.Cases.AsNoTracking()
                .Include(c => c.Lead)
                .Include(c => c.CasePictures)
                .Include(c => c.CarMMY)
                .OrderByDescending(c => c.CreatedOn)
                .Take(noCases)
                .Select(p => new RecentCasesViewModel()
                {
                    CaseNo = p.CaseNo,
                    CarDec = p.CarMMY.CarYear.ToString() + " " + p.CarMMY.CarMake + " / " + p.CarMMY.CarModel,
                    LeadFullName = p.Lead.FirstName + " " + p.Lead.LastName,
                    CaseDesc = p.CaseDesc,
                    CaseStatus = p.StatusId,
                    PhotoUrl = p.CasePictures.Where(cp => cp.PictureRank == 1).FirstOrDefault().PictureLocation,
                    ReceivedDate = p.CreatedOn,
                    CaseId = p.CaseId
                })
                .ToList();
            }
        }

        public List<Case> GetRecentCasesWithLead(int noCases)
        {
            using (var context = new fyxmeContext())
            {
                return context.Cases.AsNoTracking()
                .Include(c => c.Lead)
                .Include(c => c.CasePictures)
                .Include(c => c.CarMMY)
                .Where(c => c.Active == true)
                .OrderByDescending(c => c.CreatedOn)
                .Take(noCases)
                .ToList();
            }
        }

        public List<Case> GetCasesWithLead()
        {
            // Return All the Cases and their leads in a list
            using (var context = new fyxmeContext())
            {
                return context.Cases.AsNoTracking().Include(c => c.Lead).ToList();
            }
        }


        public Case GetCaseWithPicture(long CaseId)
        {
            // Return a Case object containing all links to the pictures related to the Case
            using (var context = new fyxmeContext())
            {
                return context.Cases.AsNoTracking().Include(c => c.CasePictures)
                    .FirstOrDefault(c => c.CaseId == CaseId);
            }
        }


        public Case GetCaseWithPicturesAndLead(long CaseId)
        {
            // Returns a Case object containing all its Lead info and all the links for the multiple assoc case pictures
            using (var context = new fyxmeContext())
            {
                return context.Cases.AsNoTracking()
                    .Include(c => c.Lead)
                    .Include(c => c.CasePictures)
                    .Include(c => c.CarMMY)
                    .Where(c => c.Active == true)
                    .FirstOrDefault(c => c.CaseId == CaseId);
            }
        }

 
        public IEnumerable GetLeadList()
        {
            // Create an object of no particular type and returns it in a IEnumerable
            // the object has some of the Lead properties
            using (var context = new fyxmeContext())
            {
                return context.Leads.AsNoTracking()
                        .Where(l => l.Active == true)
                        .OrderByDescending(l => l.LeadId)
                        .Select(l => new { l.LeadId, l.FirstName })
                        .ToList();
            }
        }


        public Case GetCaseById(long CaseId)
        {
            using (var context = new fyxmeContext())
            {
                //return context.Cases.AsNoTracking()
                //            .SingleOrDefault(c => c.CaseId == CaseId)

                return context.Cases.Find(CaseId);
            }
        }


        public int SaveUpdatedCase(Case case1)
        {
            using (var context = new fyxmeContext())
            {
                //context.Database.Log = message => SqlLogger("SaveUpdatedCase", message);
                //context.Database.Log = message => File.WriteAllText(@"C:\fyxWebAppDev\fyxme.WebApp001\SqlLogging\ll.txt").WriteLine(message);
                //context.Database.Log = SqlLog.Write;
                context.Entry(case1).State = EntityState.Modified;

                return context.SaveChanges();
                

            }
        }


        public int SaveNewCase(Case case1)
        {
            using (var context = new fyxmeContext())
            {
                context.Cases.Add(case1);
                return context.SaveChanges();
            }
        }


        public int RemoveCaseById(long CaseId)
        {
            using (var context = new fyxmeContext())
            {
                var case1 = context.Cases.Find(CaseId);
                // If a real delete then we change the state to deleted and the equivalent SQL in Delete from Case...
                //context.Entry(case1).State = EntityState.Deleted;

                case1.Active = false;
                context.Entry(case1).State = EntityState.Modified;
                return context.SaveChanges();
            }
        }


        public int SaveNewCasePicture(CasePicture cp)
        {
            using (var context = new fyxmeContext())
            {
                context.CasePictures.Add(cp);
                return context.SaveChanges();
            }
        }


        public int SaveUpdatedCasePicture(CasePicture cp)
        {
            using (var context = new fyxmeContext())
            {
                context.Entry(cp).State = EntityState.Modified;
                return context.SaveChanges();
            }
        }


        public int RemoveCasePictureById(int cpId)
        {
            using (var context = new fyxmeContext())
            {
                var cp = context.Cases.Find(cpId);
                //cp.Active = 0;
                context.Entry(cp).State = EntityState.Modified;
                return context.SaveChanges();
            }
        }


        public CasePicture GetCasePictureById(int cpId)
        {
            using (var context = new fyxmeContext())
            {
                return context.CasePictures.Find(cpId);
            }

        }

        private void SqlLogger(string fct, string SqlLog)
        {
            string folderName = @"C:\fyxWebAppDev\fyxme.WebApp001\SqlLogging";
            string fileName, filePath;
            var dt = new DateTime();
            dt = DateTime.Now;
            fileName = dt.ToString("yyyyMMddHHmmssfff") + "_" + fct;
            filePath = Path.Combine(folderName, fileName,".txt");

            //File.Write(filePath, SqlLog);

            var logFile = new StreamWriter(filePath);
            //sqlLog = logFile.Write;

            // Execute your queries here
            // ....

            logFile.Close();
        }

 
    }
}
