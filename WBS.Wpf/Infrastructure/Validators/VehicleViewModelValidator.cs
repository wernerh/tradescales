using FluentValidation;
using WBS.Wpf.ViewModel;

namespace WBS.Wpf.Infrastructure.Validators
{
    public class VehicleViewModelValidator : AbstractValidator<VehicleViewModel>
    {
        public VehicleViewModelValidator()
        {
            //RuleFor(customer => customer.FirstName).NotEmpty()
            //    .Length(1, 100).WithMessage("First Name must be between 1 - 100 characters");

            //RuleFor(customer => customer.LastName).NotEmpty()
            //    .Length(1, 100).WithMessage("Last Name must be between 1 - 100 characters");
        }
    }
}
