using System.Linq;
using TradeScales.Data.Repositories;
using TradeScales.Entities;

namespace TradeScales.Data.Extensions
{
    public static class DriverExtensions
    {
        public static bool DriverExists(this IEntityBaseRepository<Driver> driversRepository, string firstName, string lastName)
        {
            return driversRepository.GetAll()
                .Any(d => d.FirstName.ToLower() == firstName || d.LastName.ToLower() == lastName);
        }

        public static string GetDriverFullName(this IEntityBaseRepository<Driver> driversRepository, int driverId)
        {
            var driver = driversRepository.GetSingle(driverId);
            return (driver != null) ? driver.FirstName + " " + driver.LastName : "";
        }
    }
}
