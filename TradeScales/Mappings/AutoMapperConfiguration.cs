using AutoMapper;

namespace TradeScales.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(mapper =>
            {
                mapper.AddProfile<DomainToViewModelMappingProfile>();
            });
        }
    }
}
