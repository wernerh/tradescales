using System.Linq;
using WBS.Data.Repositories;
using WBS.Entities;

namespace WBS.Data.Extensions
{
    public static class HaulierExtensions
    {
        public static bool HaulierExists(this IEntityBaseRepository<Haulier> hauliersRepository, string code, string name)
        {
            return hauliersRepository.GetAll().Any(h => h.Code.ToLower() == code || h.Name.ToLower() == name);
        }

        public static Haulier GetSingleByProductCode(this IEntityBaseRepository<Haulier> hauliersRepository, string code)
        {
            return hauliersRepository.GetAll().FirstOrDefault(h => h.Code == code);
        }
    }
}
