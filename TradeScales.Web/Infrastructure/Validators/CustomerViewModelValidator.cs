﻿using FluentValidation;
using TradeScales.Web.Models;

namespace TradeScales.Web.Infrastructure.Validators
{
    public class CustomerViewModelValidator : AbstractValidator<CustomerViewModel>
    {
        public CustomerViewModelValidator()
        {
            //RuleFor(driver => driver.Code)
            //    .NotEmpty()
            //    .Length(1, 100)
            //    .WithMessage("Company code must be between 1 - 100 characters");

            //RuleFor(driver => driver.Name)
            //    .NotEmpty()
            //    .Length(1, 100)
            //    .WithMessage("Company Name must be between 1 - 100 characters");
        }
    }
}