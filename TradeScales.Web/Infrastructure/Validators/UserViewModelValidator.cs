using FluentValidation;
using TradeScales.Web.Models;

namespace TradeScales.Web.Infrastructure.Validators
{
    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        public UserViewModelValidator()
        {

        }
    }
}