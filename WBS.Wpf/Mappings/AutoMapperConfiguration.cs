using AutoMapper;
using WBS.Wpf.Mappings;

namespace WBS.WPF.Mappings
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