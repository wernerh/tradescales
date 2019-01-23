using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TradeScales.Web.Models;

namespace TradeScales.Web.Infrastructure.Validators
{
    public class DestinationViewModelValidator : AbstractValidator<DestinationViewModel>
    {
        public DestinationViewModelValidator()
        {

        }
    }
}