using System;

namespace fyxme.Data.Entities
{
    public partial class CasePicture : IModificationHistory
    {
        public long CasePictureId { get; set; }

        public byte PictureRank { get; set; }
        public string PictureName { get; set; }
        public string PictureLocation { get; set; }

        public  Case Case { get; set; }
        public long CaseId { get; set; }

        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
}
