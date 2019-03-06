using System.Linq;
using TradeScales.Data.Repositories;
using TradeScales.Entities;

namespace TradeScales.Data.Extensions
{
    public static class VehicleExtensions
    {
        public static bool VehicleExists(this IEntityBaseRepository<Vehicle> vehicleRepository, string make, string registration)
        {
            return vehicleRepository.GetAll().Any(v => v.Make == make || v.Registration == registration);
        }
    }
}
