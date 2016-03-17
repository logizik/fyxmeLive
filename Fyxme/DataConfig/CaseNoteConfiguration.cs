using System.Data.Entity.ModelConfiguration;
using fyxme.Data.Entities;

namespace fyxme.Data.Config
{
    class CaseNoteConfiguration: EntityTypeConfiguration<CaseNote>
    {
        public CaseNoteConfiguration()
        {
            Property(n => n.NoteRank).IsRequired();
            Property(n => n.NoteText).HasMaxLength(2000).IsRequired();
        }
    }
}
