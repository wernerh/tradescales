using AutoMapper;
using TradeScales.Entities;
using TradeScales.Web.Models;

namespace TradeScales.Web.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Driver, DriverViewModel>();
                         
            CreateMap<Customer, CustomerViewModel>();

            CreateMap<User, UserViewModel>();

            CreateMap<Ticket, TicketViewModel>();

            CreateMap<Product, ProductViewModel>();
        }
    }
}