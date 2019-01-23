using System.Linq;
using TradeScales.Data.Repositories;
using TradeScales.Entities;

namespace TradeScales.Data.Extensions
{
    public static class DriverExtensions
    {
        public static bool DriverExists(this IEntityBaseRepository<Driver> driversRepository, string email, string identityCard)
        {
            return driversRepository.GetAll()
                .Any(d => d.Email.ToLower() == email || d.IdentityCard.ToLower() == identityCard);
        }

        public static string GetDriverFullName(this IEntityBaseRepository<Driver> driversRepository, int driverId)
        {
            var driver = driversRepository.GetSingle(driverId);
            return (driver != null) ? driver.FirstName + " " + driver.LastName : "";
        }
    }
}
