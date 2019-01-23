using System;

namespace TradeScales.Entities
{
    public class Driver : IEntityBase
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string IdentityCard { get; set; }
        public string Mobile { get; set; }
        public string VehicleRegistration { get; set; }
        public Guid UniqueKey { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }      
    }
}
