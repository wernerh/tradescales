namespace TradeScales.Entities
{
    public class Product : IEntityBase
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
