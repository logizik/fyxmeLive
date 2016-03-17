using System;

namespace fyxme.Data.Entities
{
    public partial class CaseNote : IModificationHistory
    {
        public long CaseNoteId { get; set; }
        public int NoteRank { get; set; }
        public string NoteText { get; set; }

        public  Case Case { get; set; }
        public long CaseId { get; set; }

        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
