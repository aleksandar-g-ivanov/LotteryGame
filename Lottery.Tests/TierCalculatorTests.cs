using Lottery.Lib.Prizing;

namespace Lottery.Tests
{
    public class TierCalculatorTests
    {
        [Fact]
        public void All()
        { 
            TierCalculator tc = new();            

            var ticketPrize = tc.GetTicketPrize(10m, 5);
            var tierRevenue = tc.GetTierRevenue(50, 100);
            var tierWinnersCount = tc.GetTierWinnersCount(20, 100);

            Assert.Equal(2m, ticketPrize);
            Assert.Equal(50m, tierRevenue);
            Assert.Equal(20, tierWinnersCount);

        }
    }
}
