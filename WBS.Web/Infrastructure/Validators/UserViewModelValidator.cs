using FluentValidation;
using WBS.Web.Models;

namespace WBS.Web.Infrastructure.Validators
{
    public class UserViewModelValidator : AbstractValidator<UserViewModel>
    {
        public UserViewModelValidator()
        {

        }
    }
}