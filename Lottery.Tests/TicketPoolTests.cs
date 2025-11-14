using Lottery.Lib.Tickets;
using Lottery.Lib.Utils;
using Moq;

namespace Lottery.Tests
{
    public class TicketPoolTests
    {
    
        [Fact]
        public void AddPurchasedTickets()
        {
            // Arrange
            var mockRnd = new Mock<IRangeRandomizer>();
            var pool = new TicketPool(mockRnd.Object);
            pool.AddPurchasedTickets
            (
                new Ticket(1) { TicketPrice = 5 },
                new Ticket(2) { TicketPrice = 15 },
                new Ticket(3) { TicketPrice = 30 }
            );

            // Act
            var totalRevenue = pool.GetTotalRevenue();
            var ticketsCount = pool.GetTicketsCount();

            // Assert
            Assert.Equal(50, totalRevenue);
            Assert.Equal(3, ticketsCount);
        }

        [Fact]
        public void PickRandomTicket()
        {
            // Arrange
            var mRandomizer = new Mock<IRangeRandomizer>();
            var pool = new TicketPool(mRandomizer.Object);

            var tickets = new[]
            {
                new Ticket(1){ TicketPrice = 10 },
                new Ticket(2){ TicketPrice = 20 },
                new Ticket(3){ TicketPrice = 30 }
            };

            pool.AddPurchasedTickets(tickets);

            mRandomizer
                .Setup(r => r.GetRandomInRange(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(1);

            // Act
            var picked = pool.PickRandomTicket();

            // Assert
            Assert.Equal(2, picked.Number);
            Assert.Equal(20, picked.TicketPrice);
        }

        [Fact]
        public void PickRandomTickets()
        {
            // Arrange
            var mRandomizer = new Mock<IRangeRandomizer>();
            var pool = new TicketPool(mRandomizer.Object);

            pool.AddPurchasedTickets
            (
                new Ticket(10) { TicketPrice = 10m },
                new Ticket(20) { TicketPrice = 20m },
                new Ticket(30) { TicketPrice = 30m }
            );

            mRandomizer
                .SetupSequence(r => r.GetRandomInRange(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(0)
                .Returns(1);

            // Act
            var picked = pool.PickRandomTickets(2);

            // Assert
            Assert.Equal(2, picked.Count);
            Assert.Contains(picked, t => t.Number == 10);
            Assert.Contains(picked, t => t.Number == 20);            
        }
    }
}
