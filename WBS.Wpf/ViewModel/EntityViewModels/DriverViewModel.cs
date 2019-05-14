using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WBS.Wpf.Infrastructure.Validators;

namespace WBS.Wpf.ViewModel
{
    public class DriverViewModel : IValidatableObject
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new DriverViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}