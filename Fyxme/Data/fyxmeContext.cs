using System;
using System.Data.Entity;
using System.Linq;
using fyxme.Data.Entities;
using fyxme.Data.Config;

namespace fyxme.Data.Model
{
    public class fyxmeContext : DbContext
    {
        public fyxmeContext() : base("fyxmeDataModel")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<fyxmeContext>(new fyxmeDbInitialSeed());
            // This config below can also be done from the configuration file
            // It doesn't do the check to see if the tables ot database exists
            Database.SetInitializer(new NullDatabaseInitializer<fyxmeContext>());
        }

        public DbSet<Lead> Leads { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<CasePicture> CasePictures { get; set; }
        public DbSet<CaseNote> CaseNotes { get; set; }
        public DbSet<CarMMY> CarMMYs { get; set; }
        public DbSet<Status> Status { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CarMMYConfiguration());
            modelBuilder.Configurations.Add(new CaseConfiguration());
            modelBuilder.Configurations.Add(new CaseNoteConfiguration());
            modelBuilder.Configurations.Add(new CasePictureConfiguration());
            modelBuilder.Configurations.Add(new LeadConfiguration());
            modelBuilder.Configurations.Add(new StatusConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach (var history in this.ChangeTracker.Entries()
              .Where(e => e.Entity is IModificationHistory && (e.State == EntityState.Added ||
                      e.State == EntityState.Modified))
               .Select(e => e.Entity as IModificationHistory)
              )
            {
                history.UpdatedOn = DateTime.Now;
                if (history.CreatedOn == DateTime.MinValue)
                {
                    history.CreatedOn = DateTime.Now;
                }
            }

            foreach (var history in this.ChangeTracker.Entries()
                .Where(e => e.Entity is IModificationHistory && (e.State == EntityState.Added))
                .Select(e => e.Entity as IModificationHistory)
                )
            {
                history.Active = true;
            }

            int result = base.SaveChanges();
 
            return result;
        }

    }
}
