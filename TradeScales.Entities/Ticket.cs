using System;

namespace TradeScales.Entities
{
    public class Ticket : IEntityBase
    {
        public int ID { get; set; }
        public string LastModifiedBy { get; set; }
        public string Status { get; set; }
        public string TicketNumber { get; set; }       
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public string LastModified { get; set; }        
        public int HaulierId { get; set; }
        public int CustomerId { get; set; }
        public int DestinationId { get; set; }
        public int ProductId { get; set; }
        public int DriverId { get; set; }
        public int VehicleId { get; set; }
        public string OrderNumber { get; set; }
        public string DeliveryNumber { get; set; }
        public string SealFrom { get; set; }
        public string SealTo { get; set; }
        public double GrossWeight { get; set; }
        public double TareWeight { get; set; }
        public double NettWeight { get; set; }
       
        public virtual Haulier Haulier { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Destination Destination { get; set; }
        public virtual Product Product { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}
