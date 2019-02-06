using AutoMapper;
using TradeScales.Wpf.Mappings;

namespace TradeScales.WPF.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainToViewModelMappingProfile>();
            });
        }
    }
}