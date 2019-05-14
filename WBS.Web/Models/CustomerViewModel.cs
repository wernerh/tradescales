using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using WBS.Web.Infrastructure.Validators;

namespace WBS.Web.Models
{
    [Bind(Exclude = "UniqueKey")]
    public class CustomerViewModel : IValidatableObject
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new CustomerViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}