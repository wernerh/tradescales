using AutoMapper;
using TradeScales.Entities;
using TradeScales.Wpf.ViewModel;

namespace TradeScales.Wpf.Mappings
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