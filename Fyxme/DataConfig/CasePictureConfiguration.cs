using System.Data.Entity.ModelConfiguration;
using fyxme.Data.Entities;

namespace fyxme.Data.Config
{
    class CasePictureConfiguration:EntityTypeConfiguration<CasePicture>
    {
        public CasePictureConfiguration()
        {
            Property(p => p.PictureName).HasMaxLength(50).IsRequired();
            Property(p => p.PictureLocation).HasMaxLength(255);
        } 

    }
}
