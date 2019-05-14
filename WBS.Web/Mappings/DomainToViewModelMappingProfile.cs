using AutoMapper;
using WBS.Entities;
using WBS.Web.Models;

namespace WBS.Web.Mappings
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