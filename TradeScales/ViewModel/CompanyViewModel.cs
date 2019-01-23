using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TradeScales.Infrastructure.Validators;

namespace TradeScales.ViewModel
{
    public class CompanyViewModel : IValidatableObject
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new CompanyViewModelValidator();
            var result = validator.Validate(this);

            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
