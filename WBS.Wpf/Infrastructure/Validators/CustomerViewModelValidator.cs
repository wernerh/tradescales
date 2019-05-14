using FluentValidation;
using WBS.Wpf.ViewModel;

namespace WBS.Wpf.Infrastructure.Validators
{
    public class CustomerViewModelValidator : AbstractValidator<CustomerViewModel>
    {
        public CustomerViewModelValidator()
        {
            //RuleFor(customer => customer.Code)
            //    .NotEmpty()
            //    .Length(1, 50)
            //    .WithMessage("Customer Code must be between 1 - 50 characters");

            //RuleFor(customer => customer.Name)
            //    .NotEmpty()
            //    .Length(1, 50)
            //    .WithMessage("Customer Name must be between 1 - 50 characters");
        }
    }
}