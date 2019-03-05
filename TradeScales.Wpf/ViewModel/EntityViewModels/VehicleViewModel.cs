using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TradeScales.Wpf.Infrastructure.Validators;

namespace TradeScales.Wpf.ViewModel.EntityViewModels
{
    public class VehicleViewModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Make { get; set; }
        public string Registration { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new VehicleViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
