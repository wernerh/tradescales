using TradeScales.Entities;

namespace TradeScales.Data.Configuration
{
    public class DestinationConfiguration : EntityBaseConfiguration<Destination>
    {
        public DestinationConfiguration()
        {           
            Property(m => m.Code).IsRequired().HasMaxLength(50);
            Property(m => m.Name).IsRequired().HasMaxLength(50);             
        }
    }
}
