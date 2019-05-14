using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WBS.Web.Models;

namespace WBS.Web.Infrastructure.Validators
{
    public class ProductViewModelValidator : AbstractValidator<ProductViewModel>
    {
        public ProductViewModelValidator()
        {

        }
    }
}