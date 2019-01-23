using System;
using TradeScales.Entities;
using TradeScales.Web.Models;

namespace TradeScales.Web.Infrastructure.Extensions
{
    public static class EntitiesExtensions
    {
        public static void UpdateUser(this User user, UserViewModel userViewModel)
        {
            user.Username = userViewModel.Username;
            user.Email = userViewModel.Email;
            user.HashedPassword = userViewModel.HashedPassword;
            user.DateCreated = userViewModel.DateCreated;
        }

        public static void UpdateCustomer(this Customer company, CustomerViewModel customerViewModel)
        {
            company.Code = customerViewModel.Code;
            company.Name = customerViewModel.Name;
        }

        public static void UpdateDriver(this Driver driver, DriverViewModel driverViewModel)
        {
            driver.Code = driverViewModel.Code;
            driver.FirstName = driverViewModel.FirstName;
            driver.LastName = driverViewModel.LastName;
            driver.Email = driverViewModel.Email;
            driver.IdentityCard = driverViewModel.IdentityCard;
            driver.Mobile = driverViewModel.Mobile;
            driver.VehicleRegistration = driverViewModel.VehicleRegistration;
            driver.UniqueKey = (driverViewModel.UniqueKey == null || driverViewModel.UniqueKey == Guid.Empty) ? Guid.NewGuid() : driverViewModel.UniqueKey;
            driver.DateOfBirth = driverViewModel.DateOfBirth;
            driver.RegistrationDate = (driver.RegistrationDate == DateTime.MinValue ? DateTime.Now : driverViewModel.RegistrationDate);
        }

        public static void UpdateProduct(this Product product, ProductViewModel productViewModel)
        {
            product.Code = productViewModel.Code;
            product.Name = productViewModel.Name;
        }

        public static void UpdateDestination(this Destination destination, DestinationViewModel destinationViewModel)
        {
            destination.Code = destinationViewModel.Code;
            destination.Name = destinationViewModel.Name;
        }
        public static void UpdateHaulier(this Haulier haulier, HaulierViewModel haulierViewModel)
        {
            haulier.Code = haulierViewModel.Code;
            haulier.Name = haulierViewModel.Name;
        }
     
        public static void UpdateTicket(this Ticket ticket, TicketViewModel ticketViewModel)
        {
            ticket.LastModifiedBy = ticketViewModel.LastModifiedBy;
            ticket.Status = ticketViewModel.Status;
            ticket.TicketNumber = ticketViewModel.TicketNumber;
            ticket.TimeIn = ticketViewModel.TimeIn;
            ticket.TimeOut = ticketViewModel.TimeOut;
            ticket.LastModified = ticketViewModel.LastModified;                    
            ticket.HaulierId = ticketViewModel.HaulierId;
            ticket.CustomerId = ticketViewModel.CustomerId;
            ticket.DestinationId = ticketViewModel.DestinationId;
            ticket.ProductId = ticketViewModel.ProductId;
            ticket.DriverId = ticketViewModel.DriverId;
            ticket.OrderNumber = ticketViewModel.OrderNumber;
            ticket.DeliveryNumber = ticketViewModel.DeliveryNumber;
            ticket.SealFrom = ticketViewModel.SealFrom;
            ticket.SealTo = ticketViewModel.SealTo;
            ticket.GrossWeight = ticketViewModel.GrossWeight;
            ticket.TareWeight = ticketViewModel.TareWeight;
            ticket.NettWeight = ticketViewModel.NettWeight;
        }
    }
}