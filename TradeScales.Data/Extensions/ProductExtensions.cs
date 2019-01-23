using System.Linq;
using TradeScales.Data.Repositories;
using TradeScales.Entities;

namespace TradeScales.Data.Extensions
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
