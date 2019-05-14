using System.Data.Entity.ModelConfiguration;
using WBS.Entities;

namespace WBS.Data.Configuration
{
    public class EntityBaseConfiguration<T> : EntityTypeConfiguration<T> where T : class, IEntityBase
    {
        public EntityBaseConfiguration()
        {
            HasKey(e => e.ID);
        }
    }
}
