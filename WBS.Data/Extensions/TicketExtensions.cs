using System.Linq;
using WBS.Data.Repositories;
using WBS.Entities;

namespace WBS.Data.Extensions
{
    public static class TicketExtensions
    {
        public static bool TicketExists(this IEntityBaseRepository<Ticket> ticketsRepository, string ticketNumber)
        {
            return ticketsRepository.GetAll().Any(t => t.TicketNumber == ticketNumber);
        }

        public static Ticket GetSingleByTicketCode(this IEntityBaseRepository<Ticket> ticketsRepository, string ticketNumber)
        {
            return ticketsRepository.GetAll().FirstOrDefault(t => t.TicketNumber == ticketNumber);
        }
    }
}
