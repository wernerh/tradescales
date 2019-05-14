using AutoMapper;
using WBS.Entities;
using WBS.Wpf.ViewModel;

namespace WBS.Wpf.Mappings
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