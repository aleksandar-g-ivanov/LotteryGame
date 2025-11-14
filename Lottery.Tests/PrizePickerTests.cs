using Lottery.Lib.Configuration;
using Lottery.Lib.Core;
using Lottery.Lib.Prizing;
using Lottery.Lib.Prizing.Enums;
using Lottery.Lib.Tickets;
using Moq;
using System.Net.Sockets;

namespace Lottery.Tests
{
    public class PrizePickerTests
    {
        [Fact]
        public void Prize1st()
        {
            // Arrange
            var ticket = new Ticket(1)
            {
                PlayerId = 1,
                TicketPrice = 10
            };

            var ticketList = new List<Ticket>() { ticket };

            //1 . Config

            var config = new Mock<Config>();
            config.Setup(cfg => cfg.Prize.GrandPrize.WinnersCount).Returns(1);
            config.Setup(cfg => cfg.Prize.GrandPrize.PercentsFromRevenue).Returns(50);

            var pool = new Mock<ITicketPool>();
            pool.Setup(p => p.PickRandomTickets(It.IsAny<int>())).Returns(ticketList);

            var tierCalc = new Mock<ITierCalculator>();
            tierCalc.Setup(tc => tc.GetTicketPrize(It.IsAny<decimal>(), It.IsAny<int>())).Returns(ticket.TicketPrice);

            var prizePicker = new PrizePicker(config.Object, pool.Object, tierCalc.Object);

            //Act
            var winner = prizePicker.Prize1st();           

            // Assert
            Assert.NotNull(winner);
            Assert.Equal(WinType.GrandPrize, winner.WinType);
            Assert.Equal(ticket.TicketPrice, winner.WinningPrize);
            
        }

        [Fact]
        public void Prize2nd()
        {
            // Arrange
            var tickets = new List<Ticket>()
            {
                new Ticket(1){ TicketPrice = 10 },
                new Ticket(2){ TicketPrice = 20 },
                new Ticket(3){ TicketPrice = 30 }
            };

            decimal TicketPrize = 6;
            decimal TierRevenue = 18;
            int TierWinnersCount = 3;


            var config = new Mock<Config>();
            config.Setup(cfg => cfg.Prize.GrandPrize.WinnersCount).Returns(It.IsAny<int>);
            config.Setup(cfg => cfg.Prize.GrandPrize.PercentsFromRevenue).Returns(It.IsAny<int>);
            config.Setup(cfg => cfg.Prize.Tier2.PercentsWinningTickets).Returns(It.IsAny<int>);
            

            var pool = new Mock<ITicketPool>();
            pool.Setup(p => p.PickRandomTickets(It.IsAny<int>())).Returns(tickets);

            var tierCalc = new Mock<ITierCalculator>();
            tierCalc.Setup(tc => tc.GetTierWinnersCount(It.IsAny<int>(), It.IsAny<int>())).Returns(TierWinnersCount);
            tierCalc.Setup(tc => tc.GetTierRevenue(It.IsAny<int>(), It.IsAny<decimal>())).Returns(TierRevenue);
            tierCalc.Setup(tc => tc.GetTicketPrize(It.IsAny<decimal>(), It.IsAny<int>())).Returns(TicketPrize);

            var prizePicker = new PrizePicker(config.Object, pool.Object, tierCalc.Object);

            //Act
            var winners = prizePicker.Prize2nd();
            

            // Assert
            Assert.NotNull(winners);
            Assert.Equal(TierWinnersCount, winners.Count);
            Assert.All(winners, winnerTicket=> Assert.Equal(WinType.SecondTier, winnerTicket.WinType));
            Assert.All(winners, winnerTicket => Assert.Equal(TicketPrize, winnerTicket.WinningPrize));
        }
    }
}
