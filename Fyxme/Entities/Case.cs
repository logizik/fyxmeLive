using System;
using System.Collections.Generic;

namespace fyxme.Data.Entities
{
    public partial class Case : IModificationHistory
    {
        public Case()
        {
            //CaseNotes = new HashSet<CaseNote>();
            CasePictures = new HashSet<CasePicture>();
        }

        public long CaseId { get; set; }
        public long CaseNo { get; set; }

        public string CaseDesc { get; set; }
        public int? SalesRepId { get; set; }

        public CarMMY CarMMY { get; set; }
        public int? CarMMYId { get; set; }
        public virtual Lead Lead { get; set; }
        public long LeadId { get; set; }
        public virtual Status Status { get; set; }
        public EStatus StatusId { get; set; }

        public virtual ICollection<CaseNote> CaseNotes { get; set; }
        public virtual ICollection<CasePicture> CasePictures { get; set; }

        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
}
