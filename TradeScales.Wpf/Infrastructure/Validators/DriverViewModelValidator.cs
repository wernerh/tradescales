using FluentValidation;
using System;
using TradeScales.Wpf.ViewModel;

namespace TradeScales.Wpf.Infrastructure.Validators
{
    public class DriverViewModelValidator : AbstractValidator<DriverViewModel>
    {
        public DriverViewModelValidator()
        {
            RuleFor(customer => customer.FirstName).NotEmpty()
                 .Length(1, 100).WithMessage("First Name must be between 1 - 100 characters");

            RuleFor(customer => customer.LastName).NotEmpty()
                .Length(1, 100).WithMessage("Last Name must be between 1 - 100 characters");
        }
    }
}