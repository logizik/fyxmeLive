using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using fyxme.Data.Entities;


namespace fyxme.Data.Model
{
    public class LeadManagementRepository : ILeadManagementRepository
    {
        private fyxmeContext _context;

        public LeadManagementRepository(fyxmeContext context)
        {
            _context = context;
        }

        public List<Case> GetRecentCasesWithLead(int noCases)
        {
           

            return _context.Cases.AsNoTracking()
                .Include(c => c.Lead)
                .Include(c => c.CasePictures)
                .OrderByDescending(c => c.CreatedOn)
                .Take(noCases)
                .ToList();

        }

    }
}
