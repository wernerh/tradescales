using FluentValidation;
using WBS.Web.Models;

namespace WBS.Web.Infrastructure.Validators
{
    public class TicketViewModelValidator : AbstractValidator<TicketViewModel>
    {
        public TicketViewModelValidator()
        {
            
        }
    }
}