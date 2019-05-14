using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WBS.Web.Infrastructure.Validators;

namespace WBS.Web.Models
{
    public class UserViewModel : IValidatableObject
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new UserViewModelValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}