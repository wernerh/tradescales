using TradeScales.Entities;

namespace TradeScales.Data.Configuration
{
    public class ProductConfiguration : EntityBaseConfiguration<Product>
    {
        public ProductConfiguration()
        {           
            Property(m => m.Code).IsRequired().HasMaxLength(50);
            Property(m => m.Name).IsRequired().HasMaxLength(50);             
        }
    }
}
