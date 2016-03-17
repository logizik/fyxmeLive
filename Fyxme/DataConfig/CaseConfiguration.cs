using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using fyxme.Data.Entities;

namespace fyxme.Data.Config
{
    class CaseConfiguration: EntityTypeConfiguration<Case>
    {
        public CaseConfiguration()
        {
            Property(d => d.CaseNo)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .HasColumnAnnotation("Index",
                        new IndexAnnotation(new IndexAttribute("AK_Case_CaseNo") { IsUnique = true }));

            Property(d => d.CarMMYId).IsOptional();

            Property(d => d.StatusId).HasColumnName("CaseStatusId").IsOptional();

            Property(d => d.SalesRepId).IsOptional();

            Property(d => d.CaseDesc).HasMaxLength(550).IsOptional();

            Property(d => d.UpdatedBy).IsOptional();

           
        }
    }
}
