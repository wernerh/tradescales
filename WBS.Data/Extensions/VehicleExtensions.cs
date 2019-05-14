using System.Linq;
using WBS.Data.Repositories;
using WBS.Entities;

namespace WBS.Data.Extensions
{
    public static class VehicleExtensions
    {
        public static bool VehicleExists(this IEntityBaseRepository<Vehicle> vehicleRepository, string make, string registration)
        {
            return vehicleRepository.GetAll().Any(v => v.Make == make || v.Registration == registration);
        }
    }
}
