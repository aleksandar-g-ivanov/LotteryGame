using Lottery.Lib.Prizing.Enums;

namespace Lottery.Lib.Tickets
{
    public class Ticket
    {
        public Ticket(int ticketNumber)
        {
            Number = ticketNumber;
        }

        public int PlayerId { get; set; }
        public decimal TicketPrice { get; set; }
        public int Number { get; set; }
    }

    public class WinningTicket : Ticket
    {
        public WinningTicket(int number) : base(number)
        {
            WinType = WinType.None;
        }
        public bool IsWinner => WinType != WinType.None;
        public WinType WinType { get; set; }
        public decimal WinningPrize { get; set; }
        public static WinningTicket FromTicket(Ticket ticket)
        {
            var winTicket = new WinningTicket(ticket.Number);
            winTicket.PlayerId = ticket.PlayerId;
            winTicket.TicketPrice = ticket.TicketPrice;
            return winTicket;
        }
    }
}
