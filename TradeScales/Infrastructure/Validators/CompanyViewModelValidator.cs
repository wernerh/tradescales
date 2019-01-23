using FluentValidation;
using TradeScales.ViewModel;

namespace TradeScales.Infrastructure.Validators
{
    public class CompanyViewModelValidator : AbstractValidator<CompanyViewModel>
    {
        public CompanyViewModelValidator()
        {
            RuleFor(company => company.Code)
                .NotEmpty()
                .Length(8)
                .WithMessage("Invalid Company Code");
        }
    }
}
