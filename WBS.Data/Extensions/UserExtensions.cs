using System.Linq;
using WBS.Data.Repositories;
using WBS.Entities;

namespace WBS.Data.Extensions
{
    public static class UserExtensions
    {
        public static bool UserExists(this IEntityBaseRepository<User> usersRepository, string email, string username)
        {
            return usersRepository.GetAll()
                 .Any(u => u.Email.ToLower() == email || u.Username.ToLower() == username);
        }

        public static User GetSingleByUsername(this IEntityBaseRepository<User> userRepository, string username)
        {
            return userRepository.GetAll().FirstOrDefault(x => x.Username == username);
        }
    }
}
