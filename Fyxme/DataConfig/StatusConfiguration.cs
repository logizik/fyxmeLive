using System.Data.Entity.ModelConfiguration;
using fyxme.Data.Entities;

namespace fyxme.Data.Config
{
    class StatusConfiguration : EntityTypeConfiguration<Status>
    {
        public  StatusConfiguration()
        {
           // Property(s => s.ParentStatusId).IsOptional();
            Property(s => s.StatusTypeId).IsRequired();
            Property(s => s.StatusName).HasMaxLength(50).IsRequired();
        }
    }
}
