using System;
using fyxme.Data.Entities;

namespace fyxme.Data.Model
{
    public class RecentCasesViewModel
    {
        public RecentCasesViewModel()
        {
        }

        public string CarDec { get; set; }
        public string CaseDesc { get; set; }
        public long CaseId { get; internal set; }
        public long CaseNo { get; set; }
        public EStatus CaseStatus { get; set; }
        public string LeadFullName { get; set; }
        public string  PhotoUrl { get; set; }
        public DateTime ReceivedDate { get; set; }
    }
}