namespace WBS.Entities
{
    public class StatusMessage : IEntityBase
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
    }
}
