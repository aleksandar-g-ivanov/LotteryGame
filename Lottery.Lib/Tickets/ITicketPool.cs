namespace Lottery.Lib.Tickets
{
    public interface ITicketPool
    {
        int GetTicketsCount();
        decimal GetTotalRevenue();
        void AddPurchasedTickets(params Ticket[] tickets);
        Ticket PickRandomTicket();
        List<Ticket> PickRandomTickets(int count);
    }
}
