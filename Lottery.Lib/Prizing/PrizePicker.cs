using Lottery.Lib.Configuration;
using Lottery.Lib.Prizing;
using Lottery.Lib.Prizing.Enums;
using Lottery.Lib.Tickets;

namespace Lottery.Lib.Core
{
    public class PrizePicker : IPrizePicker
    {
        readonly Config _config;
        readonly ITicketPool _pool;
        readonly ITierCalculator _tierCalc;
        decimal _totalRevenue;
        int _ticketCount;

        public PrizePicker(Config config, ITicketPool pool, ITierCalculator tierCalc)
        {
            _config = config;
            _pool = pool;
            _tierCalc = tierCalc;
            _totalRevenue = 0m;
            _ticketCount = 0;
        }

        public decimal TotalRevenue
        {
            get
            {
                if (_totalRevenue == 0)
                {
                    _totalRevenue = _pool.GetTotalRevenue();
                }
                return _totalRevenue;
            }
        }
        public int TicketsCount
        {
            get
            {
                if (_ticketCount == 0)
                {
                    _ticketCount = _pool.GetTicketsCount();
                }
                return _ticketCount;
            }
        }

        public WinningTicket Prize1st()
        {
            WinningTicket winner = default;
            int tierWinnersCount = _config.Prize.GrandPrize.WinnersCount;
            decimal tierRevenue = _tierCalc.GetTierRevenue(_config.Prize.GrandPrize.PercentsFromRevenue, TotalRevenue);
            winner = PickWinners(tierWinnersCount, tierRevenue, WinType.GrandPrize).FirstOrDefault();
            return winner;
        }
        public List<WinningTicket> Prize2nd()
        {
            List<WinningTicket> winners = GetTierWinners
            (
                WinType.SecondTier,
                _config.Prize.Tier2.PercentsWinningTickets,
                _config.Prize.Tier2.PercentsFromRevenue
            );

            return winners;
        }
        public List<WinningTicket> Prize3rd()
        {
            List<WinningTicket> winners = GetTierWinners
            (
                 WinType.ThirdTier,
                _config.Prize.Tier3.PercentsWinningTickets,
                _config.Prize.Tier3.PercentsFromRevenue
            );

            return winners;
        }

        List<WinningTicket> GetTierWinners(WinType winType, int tierPercentWinningTickets, int tierPercentFromRevenue)
        {
            List<WinningTicket> winners = new();

            int tierWinnersCount = _tierCalc.GetTierWinnersCount(tierPercentWinningTickets, TicketsCount);
            decimal tierRevenue = _tierCalc.GetTierRevenue(tierPercentFromRevenue, TotalRevenue);

            winners = PickWinners(tierWinnersCount, tierRevenue, winType);

            return winners;
        }
        List<WinningTicket> PickWinners(int tierWinnersCount, decimal tierRevenue, WinType winnerType)
        {
            List<WinningTicket> winners = new();
            List<Ticket> pickedWinners = _pool.PickRandomTickets(tierWinnersCount);
            decimal ticketPrize = _tierCalc.GetTicketPrize(tierRevenue, pickedWinners.Count);

            foreach (Ticket ticket in pickedWinners)
            {
                WinningTicket winner = WinningTicket.FromTicket(ticket);
                winner.WinningPrize = ticketPrize;
                winner.WinType = winnerType;

                winners.Add(winner);
            }

            return winners;
        }
    }
}
