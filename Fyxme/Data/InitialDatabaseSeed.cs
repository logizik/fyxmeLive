using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using fyxme.Data.Entities;

namespace fyxme.Data.Model
{
    public class fyxmeDbInitialSeed : DropCreateDatabaseIfModelChanges<fyxmeContext>
    {
       public fyxmeDbInitialSeed()
        { }

        protected override void Seed(fyxmeContext context)
        {
            if (context.Leads.Count() == 0)
            {
                try
                {
                    InsertLead();
                    InsertCase();
                    InsertMultipleCasePicture();
                    InsertCaseNote();
                    InsertLeadCasePictures();
                    InsertGraph_Danny();
                    InsertCaseNotes_Danny();
                }
                catch (Exception ex)
                {
                    // TODO LOG
                    ex.ToString();
                }
            }
        }

        public static int InsertLead()
        {
            var lead1 = new Lead
            {
                FirstName = "Sal",
                LastName = "Mou",
                Email = "salmou@momo.com",
                PhoneNumber = "1234567234",
                ZipCode = "12345",
                StatusId = (int)EStatus.Received,
                Origin = "site",
                //Active = true,
                CreatedBy = 0,
            };

            using (var context = new fyxmeContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Leads.Add(lead1);
                return context.SaveChanges();
            }
        }

        public static int InsertCase()
        {
            var case1 = new Case
            {
                LeadId = 1,
                //CarRank
                CarMMYId = 55,
                CaseDesc = "bbb",
                StatusId = (int)EStatus.Received,
                //SalesRepId 
                //Active = true,
                CreatedBy = 0,
                //UpdatedBy
            };

            using (var context = new fyxmeContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Cases.Add(case1);
                return context.SaveChanges();
            }
        }

        public static int InsertMultipleCasePicture()
        {
            var casePicture1 = new CasePicture
            {
                CaseId = 1,
                PictureRank = 1,
                PictureName = "abcdef",
                PictureLocation = "some path",
                Active = true,
                CreatedBy = 0,
                //UpdatedBy = "",
            };

            var casePicture2 = new CasePicture
            {
                CaseId = 1,
                PictureRank = 2,
                PictureName = "abcdef 2",
                PictureLocation = "some path",
                Active = true,
                CreatedBy = 0,
                //UpdatedBy = "",
            };

            using (var context = new fyxmeContext())
            {
                context.Database.Log = Console.WriteLine;
                context.CasePictures.AddRange(new List<CasePicture> { casePicture1, casePicture2 });
                return context.SaveChanges();
            }
        }

        public static int InsertCaseNote()
        {
            var caseNote1 = new CaseNote
            {

                CaseId = 1,
                NoteRank = 1,
                NoteText = "This is some note for Case 1, Lead 1",
                //Active = true,
                CreatedBy = 0,
                //UpdatedBy = "",
            };

            using (var context = new fyxmeContext())
            {
                context.Database.Log = Console.WriteLine;
                context.CaseNotes.Add(caseNote1);
                return context.SaveChanges();
            }
        }

        private static int InsertLeadCasePictures()
        {
            // INSERTING OF a GRAPH OF RELATED OBJECTS - fyxme App It is a Form submission simulation
            // 1 Lead contains multiple cases and One Case contains multiple CasePictures.
            // In the following there are: One LEAD having ONE CASE having 2 PICTURES
            // NO NEED to Set the Foreign Keys properties in case and in casepicture because all of teh objects are in the same context!
            // Also, all 4 insert are made inside of a single transaction
            using (var context = new fyxmeContext())
            {
                context.Database.Log = Console.WriteLine;

                var lead1 = new Lead
                {
                    FirstName = "Ghislain",
                    LastName = "Tr",
                    Email = "g1@fyxme.com",
                    PhoneNumber = "987654321",
                    ZipCode = "54321",
                    StatusId = (int)EStatus.Received,
                    Origin = "site",
                    // Active = true,
                    CreatedBy = 0,
                };
                var case1 = new Case
                {
                    //LeadId = 1,
                    //CarRank
                    CarMMYId = 74,
                    CaseDesc = "bla bla for Lead 2",
                    StatusId = (int)EStatus.Received,
                    //SalesRepId 
                    // Active = true,
                    CreatedBy = 0,
                    //UpdatedBy

                };
                var casep1 = new CasePicture
                {
                    //CaseId = 2,
                    PictureRank = 1,
                    PictureName = "Picture1",
                    PictureLocation = "some path 1",
                    //Active = true,
                    CreatedBy = 0,
                    //UpdatedBy = "",
                };
                var casep2 = new CasePicture
                {
                    //CaseId = 2,
                    PictureRank = 2,
                    PictureName = "Picture2",
                    PictureLocation = "some path 2",
                    // Active = true,
                    CreatedBy = 0,
                    //UpdatedBy = "",
                };

                // Cases List is not instanciated inside of Lead
                lead1.Cases = new List<Case>() { case1 };
                // No need to instanciate a new collection object, it is done in Case' constructor
                case1.CasePictures.Add(casep1);
                case1.CasePictures.Add(casep2);
                context.Leads.Add(lead1);
                return context.SaveChanges();
            }

        }

        private static int InsertGraph_Danny()
        {
            // INSERTING OF a GRAPH OF RELATED OBJECTS - fyxme App It is a Form submission simulation
            // All objects are within the main object which is the Lead
            // Then we only attach the Lead object to the context and the context will insert all related data in 3 tables: Leads, Cases and CasePictures
            // Below is a Test Graph having 1 Lead Danny with 2 related cases, Case1 with 2 children Pictures and Case 2 with 1 picture only!
            using (var context = new fyxmeContext())
            {
                context.Database.Log = Console.WriteLine;

                var lead1 = new Lead
                {
                    FirstName = "Danny",
                    LastName = "Gho",
                    Email = "danny@fyxme.com",
                    PhoneNumber = "987654321",
                    ZipCode = "54321",
                    StatusId = (int)EStatus.Received,
                    Origin = "site",
                    // Active = true,
                    CreatedBy = 0,
                    Cases = new List<Case>()
                    {
                        new Case()

                        {
                            CarMMYId = 99,
                            CaseDesc = "bla bla for Graph2 - Danny - First Case",
                            StatusId = (int)EStatus.Received,
                            CreatedBy = 0,
                            CasePictures = new List<CasePicture>()
                            {
                                new CasePicture()
                                {
                                    PictureRank = 1,
                                    PictureName = "Graph2 - Danny - Case 1 - Picture 1.jpg",
                                    PictureLocation = @"C:\fyxWebAppDev\fyxmeTestPictures",
                                    CreatedBy = 0,
                                },

                                new CasePicture()
                                {
                                    PictureRank = 2,
                                    PictureName = "Graph2 - Danny - Case 1 - Picture 2.jpg",
                                    PictureLocation = @"C:\fyxWebAppDev\fyxmeTestPictures",
                                    CreatedBy = 0
                                }
                            }
                        },

                        new Case()
                        {
                            CarMMYId = 199,
                            CaseDesc = "Graph2 - Danny - Second Cup",
                            StatusId = (int)EStatus.Received,
                            CreatedBy = 0,
                            CasePictures = new List<CasePicture>()
                            {
                                new CasePicture()
                                {
                                    PictureRank = 1,
                                    PictureName = "Graph2 - Danny - Case 2 - Picture 1.jpg",
                                    PictureLocation = @"C:\fyxWebAppDev\fyxmeTestPictures",
                                    CreatedBy = 0,
                                }
                            }
                        }
                    }
                };

                context.Leads.Add(lead1);
                return context.SaveChanges();



            }
        }

        private static int InsertCaseNotes_Danny()
        {
            var cn1 = new CaseNote()
            {

                CaseId = 3,
                NoteRank = 1,
                NoteText = "This is Sales Rep 01 and this is my note about Case No 15504",

            };

            var cn2 = new CaseNote()
            {
                CaseId = 3,
                NoteRank = 2,
                NoteText = "The customer is an Ass!!!"
            };

            var cn3 = new CaseNote()
            {
                CaseId = 4,
                NoteText = "The customer loves his red truck so much"
            };

            using (var context = new fyxmeContext())
            {
                context.Database.Log = Console.WriteLine;
                context.CaseNotes.AddRange(new List<CaseNote>() { cn1, cn2, cn3 });
                return context.SaveChanges();
            }

        }

    }
}