using Lottery.Lib.Configuration;
using Lottery.Lib.Tickets;
using Lottery.Lib.Utils;

namespace Lottery.Lib.Factories
{
    public class TicketFactory : IFactory<Ticket>
    {
        readonly HashSet<int> _generatedTicketNumbers;
        readonly Config _config;
        readonly IRangeRandomizer _rnd;
        public TicketFactory(IRangeRandomizer rnd, Config config)
        {
            _generatedTicketNumbers = new();
            _config = config;
            _rnd = rnd;
        }

        public Ticket Create()
        {
            int ticketNumber = _generatedTicketNumbers.PickIndexInRange
            (
                _rnd, 
                _config.Ticket.MinTicketNumber,
                _config.Ticket.MaxTicketNumber
            );
            var newTicket = new Ticket(ticketNumber);
            newTicket.TicketPrice = _config.Ticket.TicketPrice;
            return newTicket;
        }      
    }
}
