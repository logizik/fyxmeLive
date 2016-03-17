using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using fyxme.Data.Entities;

namespace fyxme.Data.Model
{
    public interface ILeadManagementRepository
    {
        List<Case> GetRecentCasesWithLead(int noCases);
    }
}


