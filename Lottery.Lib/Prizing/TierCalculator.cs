namespace Lottery.Lib.Prizing
{

    public class TierCalculator : ITierCalculator
    {        
        public TierCalculator()
        {            
        }

        public decimal GetTicketPrize(decimal tierRevenue, int tierTicketsCount)
        {
            decimal ticketPrize = Math.Round(tierRevenue / tierTicketsCount, 2);
            return ticketPrize;
        }
        public int GetTierWinnersCount(int tierPercentsOfTickets, int ticketsCount)
        {
            int tierWinnersCount = (int)Math.Round((decimal)(ticketsCount * tierPercentsOfTickets) / 100);
            return tierWinnersCount;
        }
        public decimal GetTierRevenue(int tierPercentsFromRevenue, decimal totalRevenue)
        {
            decimal tierRevenue = totalRevenue * tierPercentsFromRevenue / 100;
            return tierRevenue;
        }
    }
}
