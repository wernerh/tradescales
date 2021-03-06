//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TradeScales.Wpf
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ticket
    {
        public int ID { get; set; }
        public string LastModifiedBy { get; set; }
        public string Status { get; set; }
        public string TicketNumber { get; set; }
        public System.DateTime TimeIn { get; set; }
        public System.DateTime TimeOut { get; set; }
        public System.DateTime LastModified { get; set; }
        public int HaulierId { get; set; }
        public int CustomerId { get; set; }
        public int DestinationId { get; set; }
        public int ProductId { get; set; }
        public int DriverId { get; set; }
        public string OrderNumber { get; set; }
        public string DeliveryNumber { get; set; }
        public string SealFrom { get; set; }
        public string SealTo { get; set; }
        public double GrossWeight { get; set; }
        public double TareWeight { get; set; }
        public double NettWeight { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Destination Destination { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual Haulier Haulier { get; set; }
        public virtual Product Product { get; set; }
    }
}
