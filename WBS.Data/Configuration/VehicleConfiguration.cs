using WBS.Entities;

namespace WBS.Data.Configuration
{
    public class VehicleConfiguration : EntityBaseConfiguration<Vehicle>
    {
        public VehicleConfiguration()
        {
            Property(m => m.Code).IsRequired().HasMaxLength(50);
            Property(m => m.Make).IsRequired().HasMaxLength(50);
            Property(m => m.Registration).IsRequired().HasMaxLength(50);
        }
    }
}
