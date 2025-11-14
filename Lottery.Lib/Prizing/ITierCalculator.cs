namespace Lottery.Lib.Prizing
{
    public interface ITierCalculator
    {
        decimal GetTicketPrize(decimal tierRevenue, int tierTicketsCount);
        int GetTierWinnersCount(int tierPercentsOfTickets, int ticketsCount);
        decimal GetTierRevenue(int tierPercentsFromRevenue, decimal totalRevenue);
    }
}
