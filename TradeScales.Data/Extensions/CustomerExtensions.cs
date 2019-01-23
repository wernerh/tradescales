using System.Linq;
using TradeScales.Data.Repositories;
using TradeScales.Entities;

namespace TradeScales.Data.Extensions
{
    public static class CustomerExtensions
    {
        public static bool CustomerExists(this IEntityBaseRepository<Customer> customersRepository, string code)
        {
            return customersRepository.GetAll().Any(c => c.Code.ToLower() == code);
        }
    }
}
