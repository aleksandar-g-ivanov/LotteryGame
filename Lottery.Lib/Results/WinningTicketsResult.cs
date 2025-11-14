using Lottery.Lib.Tickets;

namespace Lottery.Lib.Results
{
    public class WinningTicketsResult
    {
        List<WinningTicket> _allWinners;
        public int PlayersCount { get; set; }
        public int TotalTicketsCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalPrize { get; set; }
        public decimal HouseProfit { get; set; }

        public WinningTicket GrandPrizeWinner { get; set; }
        public List<WinningTicket> SecondTierWinners { get; set; }
        public List<WinningTicket> ThirdTierWinners { get; set; }
        public List<WinningTicket> AllWinners
        {
            get
            {
                if (_allWinners == null)
                {
                    var allWinners = new List<WinningTicket>() { GrandPrizeWinner };

                    _allWinners = allWinners
                                    .Concat(SecondTierWinners)
                                    .Concat(ThirdTierWinners)
                                    .ToList();
                }

                return _allWinners;
            }
        }

        public void CalculateHouseProfit()
        {
            TotalPrize = 0;
            foreach (WinningTicket winner in AllWinners)
            {
                TotalPrize += winner.WinningPrize;
            }

            HouseProfit = TotalRevenue - TotalPrize;
        }
    }   
}
