using System.Linq;
using TradeScales.Data.Repositories;
using TradeScales.Entities;

namespace TradeScales.Data.Extensions
{
    public static class DestinationExtensions
    {
        public static bool DestinationExists(this IEntityBaseRepository<Destination> destinationsRepository, string code, string name)
        {
            return destinationsRepository.GetAll().Any(d => d.Code.ToLower() == code || d.Name.ToLower() == name);
        }

        public static Destination GetSingleByProductCode(this IEntityBaseRepository<Destination> destinationsRepository, string code)
        {
            return destinationsRepository.GetAll().FirstOrDefault(d => d.Code == code);
        }
    }
}
