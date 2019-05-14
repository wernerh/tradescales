using System;

namespace WBS.Entities
{
    public class Driver : IEntityBase
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }     
    }
}
