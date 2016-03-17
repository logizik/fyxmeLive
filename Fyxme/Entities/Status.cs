using System;
using System.Collections.Generic;

namespace fyxme.Data.Entities
{
    public partial class Status
    {
        
        public EStatus StatusId { get; set; }
        public byte StatusTypeId { get; set; }
        public string StatusName { get; set; }
        //public byte ParentStatusId { get; set; }

        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        //public virtual ICollection<Lead> Leads { get; set; }
    }

    public enum EStatus
    {
        Received,
        Rejected,
        MissingInfo,
        Quote,
        QuoteAccepted,
        QuoteRejected,
        JobPreparing,
        JobScheduled,
        JobCancelled,
        JobInProgress,
        DONE
    }
}
