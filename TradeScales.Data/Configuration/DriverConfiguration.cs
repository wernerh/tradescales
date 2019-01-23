using TradeScales.Entities;

namespace TradeScales.Data.Configuration
{
    public class DriverConfiguration : EntityBaseConfiguration<Driver>
    {
        public DriverConfiguration()
        {
            Property(d => d.Code).IsRequired().HasMaxLength(50);
            Property(d => d.FirstName).IsRequired().HasMaxLength(100);
            Property(d => d.LastName).IsRequired().HasMaxLength(100);
            Property(d => d.IdentityCard).IsRequired().HasMaxLength(50);
            Property(d => d.UniqueKey).IsRequired();
            Property(d => d.Mobile).HasMaxLength(10);
            Property(d => d.Email).IsRequired().HasMaxLength(200);
            Property(c => c.DateOfBirth).IsRequired();
            Property(d => d.VehicleRegistration).IsRequired().HasMaxLength(50);                      
        }
    }
}
