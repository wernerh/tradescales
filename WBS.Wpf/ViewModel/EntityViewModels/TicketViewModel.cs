﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WBS.Entities;
using WBS.Wpf.Infrastructure.Validators;

namespace WBS.Wpf.ViewModel
{
    public class TicketViewModel
    {
        public int ID { get; set; }
        public string LastModifiedBy { get; set; }
        public string Status { get; set; }
        public string TicketNumber { get; set; }      
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
        public DateTime LastModified { get; set; }
        public int VehicleId { get; set; }
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

        public virtual Haulier Haulier { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Destination Destination { get; set; }
        public virtual Product Product { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new TicketViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}