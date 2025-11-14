using Lottery.Lib.Utils;

namespace Lottery.Lib.Tickets
{
    public class TicketPool : ITicketPool
    {
        HashSet<int> _pickedIndexes;
        List<Ticket> _purchasedTickets;
        readonly IRangeRandomizer _randomizer;

        public TicketPool(IRangeRandomizer randomizer)
        {
            _pickedIndexes = new();
            _purchasedTickets = new();
            _randomizer = randomizer;
        }

        public int GetTicketsCount() => _purchasedTickets.Count;

        public decimal GetTotalRevenue()
        {
            decimal sum = _purchasedTickets.Sum(ticket => ticket.TicketPrice);
            return sum;
        }

        public void AddPurchasedTickets(params Ticket[] tickets)
        {
            foreach (Ticket ticket in tickets)
            {
                _purchasedTickets.Add(ticket);
            }
        }

        public Ticket PickRandomTicket()
        {
            int index = _pickedIndexes.PickIndexInRange(_randomizer, 0, _purchasedTickets.Count - 1);
            return _purchasedTickets[index];
        }

        public List<Ticket> PickRandomTickets(int count)
        {
            List<Ticket> pickedTickets = new();
            List<int> indexes = _pickedIndexes.PickCountIndexesInRange(count, _randomizer, 0, _purchasedTickets.Count - 1);
            foreach (int ticketIndex in indexes)
            {
                Ticket pickedTicket = _purchasedTickets[ticketIndex];
                pickedTickets.Add(pickedTicket);
            }

            return pickedTickets;
        }       
    }
}
