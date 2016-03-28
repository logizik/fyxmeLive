using System;
using System.Collections.Generic;

namespace fyxme.Data.Entities
{
    public partial class Technician : IModificationHistory
    {
        public long TechnicianId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ZipCode { get; set; }
        public string Resume { get; set; }
        public string ResumeLocation { get; set; }

        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
