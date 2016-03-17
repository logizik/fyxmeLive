using System;
using System.Collections.Generic;

namespace fyxme.Data.Entities
{
    public partial class Lead : IModificationHistory
    {
        public Lead()
        {
            Cases = new List<Case>();
        }

        public long LeadId { get; set; }
        public long LeadNo { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ZipCode { get; set; }
        public string Origin { get; set; }

        public Status Status { get; set; }
        public EStatus StatusId { get; set; }
        public ICollection<Case> Cases { get; set; }

        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
}
