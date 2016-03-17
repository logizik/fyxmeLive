using System;

namespace fyxme.Data.Entities
{
    public interface IModificationHistory
    {
        DateTime CreatedOn { get; set; }
        DateTime? UpdatedOn { get; set; }
        bool Active { get; set; }
        //bool IsDirty { get; set; }
    }
}
