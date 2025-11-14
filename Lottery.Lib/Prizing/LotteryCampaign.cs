using Lottery.Lib.Logging;
using Lottery.Lib.Players;
using Lottery.Lib.Prizing.Enums;
using Lottery.Lib.Results;
using Lottery.Lib.Tickets;

namespace Lottery.Lib.Core
{
    public class LotteryCampaign
    {
        readonly ITicketPool _ticketPool;
        readonly IPrizePicker _prizePicker;
        readonly IResultFormatter _formatter;
        readonly ILogger _logger;

        WinningTicketsResult _winningsResult;
        CampaignStatus _status;
        int _playersCount;
        
        public LotteryCampaign
        (
            ITicketPool ticketPool,
            IPrizePicker prizePicker,
            IResultFormatter formatter,
            ILogger logger
        )
        {
            _ticketPool = ticketPool;
            _prizePicker = prizePicker;
            _formatter = formatter;
            _logger = logger;

            Initialize();
        }

        void Initialize()
        {
            _status = CampaignStatus.Started;
            _playersCount = 0;
            CampaignID = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            _logger.Info($"Campaign {CampaignID} started.");
        }

        public string CampaignID { get; set; }
        public WinningTicketsResult WinningsResult => _winningsResult;

        public void RegisterPlayers(params Player[] players)
        {
            if (_status != CampaignStatus.Started)
            {
                _logger.Info("Campaign is already closed for registration.");
            }
            foreach (Player player in players)
            {
                _playersCount++;
                _ticketPool.AddPurchasedTickets(player.Tickets.ToArray());
                _logger.Info($"{player.Name, -10} has registered in campaign {CampaignID}.");
            }            
        }

        public void DrawWinners()
        {            
            _logger.Info("\n\nDrawing winners ...");
            _status = CampaignStatus.ClosedForRegistration;

            _winningsResult = new();
            _winningsResult.PlayersCount = _playersCount;

            _winningsResult.TotalTicketsCount = _prizePicker.TicketsCount;
            _winningsResult.TotalRevenue = _prizePicker.TotalRevenue;
            _winningsResult.GrandPrizeWinner = _prizePicker.Prize1st();
            _winningsResult.SecondTierWinners = _prizePicker.Prize2nd();
            _winningsResult.ThirdTierWinners = _prizePicker.Prize3rd();

            _winningsResult.CalculateHouseProfit();

            _status = CampaignStatus.Completed;
            _logger.Info("Campaign completed.");
        }

        public void LogResults()
        {
            _logger.Info("Results");
            string result = _formatter.Format(_winningsResult);
            _logger.Info(result);
        }
    }
}
