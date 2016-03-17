using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using fyxme.Data.Entities;

namespace fyxme.Data.Config
{
    class LeadConfiguration: EntityTypeConfiguration<Lead>
    {
        public LeadConfiguration()
        {
            Property(d => d.LeadNo)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .HasColumnAnnotation("Index",
            new IndexAnnotation(new IndexAttribute("AK_Lead_LeadNo") { IsUnique = true }));

            Property(f => f.FirstName).HasMaxLength(50).IsRequired();
            Property(f => f.LastName).HasMaxLength(100).IsRequired();
            Property(f => f.Email).HasMaxLength(320).IsRequired();
            Property(f => f.PhoneNumber).HasMaxLength(20).IsRequired();
            Property(f => f.ZipCode).HasMaxLength(10).IsRequired();
            Property(f => f.Origin).HasMaxLength(30).IsOptional();
            Property(f => f.StatusId).IsOptional();

        }
    }
}
