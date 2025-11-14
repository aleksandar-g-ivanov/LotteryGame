using Lottery.Lib.Tickets;
using System.ComponentModel.DataAnnotations;

namespace Lottery.Lib.Players
{
    public class Player
    {
        List<Ticket> _tickets;
      
        public Player()
        {
            _tickets = new();
        }

        public PlayerType PlayerType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Ballance { get; set; }
        public int TicketsCount => _tickets.Count;
        public List<Ticket> Tickets => _tickets;
        public void PurchaseTickets(Ticket[] tickets)
        {
            ValidatePurchase(tickets);
            foreach (Ticket ticket in tickets)
            {
                ticket.PlayerId = Id;
                _tickets.Add(ticket);
                Ballance -= ticket.TicketPrice;
            }
        }
        void ValidatePurchase(Ticket[] tickets)
        {
            decimal totalTicketsPrice = tickets.Sum(t => t.TicketPrice);
            if (totalTicketsPrice > Ballance)
            {
                throw new ValidationException("Player balance is not enough to purchase tickets.");
            }
        }
    }   
}
