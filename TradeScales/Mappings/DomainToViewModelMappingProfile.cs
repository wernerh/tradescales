using AutoMapper;
using TradeScales.Entities;
using TradeScales.ViewModel;

namespace TradeScales.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
    
        public DomainToViewModelMappingProfile()
        {
            CreateMap<CompanyViewModel, Company>()
                .ForMember(vm => vm.Code, map => map.MapFrom(m => m.Code))
                .ForMember(vm => vm.Name, map => map.MapFrom(m => m.Name));
        }     
    }
}