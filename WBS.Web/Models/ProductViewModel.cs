using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WBS.Web.Infrastructure.Validators;

namespace WBS.Web.Models
{
    public class ProductViewModel : IValidatableObject
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }    
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new ProductViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}