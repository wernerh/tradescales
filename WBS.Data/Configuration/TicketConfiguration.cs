using WBS.Entities;

namespace WBS.Data.Configuration
{
    public class TicketConfiguration : EntityBaseConfiguration<Ticket>
    {
        public TicketConfiguration()
        {           
            Property(m => m.TicketNumber).IsRequired().HasMaxLength(50);
            Property(m => m.TimeIn);
            Property(m => m.TimeOut);
            Property(m => m.LastModified);           
            Property(m => m.HaulierId).IsRequired();
            Property(m => m.CustomerId).IsRequired();
            Property(m => m.DestinationId).IsRequired();
            Property(m => m.ProductId).IsRequired();
            Property(m => m.DriverId).IsRequired();
            Property(m => m.VehicleId).IsRequired();
            Property(m => m.OrderNumber).IsRequired();
            Property(m => m.DeliveryNumber).IsRequired();
            Property(m => m.LastModifiedBy).IsRequired();
            Property(m => m.Status).IsRequired();
        }
    }
}
