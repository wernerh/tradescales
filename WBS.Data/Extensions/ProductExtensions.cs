using System.Linq;
using WBS.Data.Repositories;
using WBS.Entities;

namespace WBS.Data.Extensions
{
    public static class ProductExtensions
    {
        public static bool ProductExists(this IEntityBaseRepository<Product> productsRepository, string code, string name)
        {
            return productsRepository.GetAll().Any(p => p.Code.ToLower() == code || p.Name.ToLower() == name);
        }

        public static Product GetSingleByProductCode(this IEntityBaseRepository<Product> productsRepository, string code)
        {
            return productsRepository.GetAll().FirstOrDefault(x => x.Code == code);
        }
    }
}
