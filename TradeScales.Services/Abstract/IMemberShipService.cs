using System.Collections.Generic;
using TradeScales.Entities;
using TradeScales.Services.Utilities;

namespace TradeScales.Services.Abstract
{
    public interface IMembershipService
    {
        MembershipContext ValidateUser(string username, string password);
        User CreateUser(string username, string email, string password, int[] roles);
        User GetUser(int userId);
        List<Role> GetUserRoles(string username);
    }
}
