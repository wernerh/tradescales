using System.Linq;
using WBS.Data.Repositories;
using WBS.Entities;

namespace WBS.Data.Extensions
{
    public static class CustomerExtensions
    {
        public static bool CustomerExists(this IEntityBaseRepository<Customer> customersRepository, string code)
        {
            return customersRepository.GetAll().Any(c => c.Code.ToLower() == code);
        }
    }
}
