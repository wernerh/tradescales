using WBS.Entities;

namespace WBS.Data.Configuration
{
    public class DriverConfiguration : EntityBaseConfiguration<Driver>
    {
        public DriverConfiguration()
        {
            Property(d => d.Code).IsRequired().HasMaxLength(50);
            Property(d => d.FirstName).IsRequired().HasMaxLength(100);
            Property(d => d.LastName).IsRequired().HasMaxLength(100);                    
        }
    }
}
