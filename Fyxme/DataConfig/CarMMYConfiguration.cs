using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using fyxme.Data.Entities;

namespace fyxme.Data.Config
{
    public class CarMMYConfiguration : EntityTypeConfiguration<CarMMY>
    {
        public CarMMYConfiguration()
        {
            ToTable("CarMMY");

            Property(c => c.CarMake)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("AK_CarMMY", 1) { IsUnique = true }));


            Property(c => c.CarModel)
                .HasMaxLength(250)
                .IsRequired()
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("AK_CarMMY", 2) { IsUnique = true }));

            Property(c => c.CarYear)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("AK_CarMMY", 3) { IsUnique = true }));


        }
    }
}
