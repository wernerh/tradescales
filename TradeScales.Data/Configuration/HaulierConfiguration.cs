using TradeScales.Entities;

namespace TradeScales.Data.Configuration
{
    public class HaulierConfiguration : EntityBaseConfiguration<Haulier>
    {
        public HaulierConfiguration()
        {           
            Property(m => m.Code).IsRequired().HasMaxLength(50);
            Property(m => m.Name).IsRequired().HasMaxLength(50);             
        }
    }
}
