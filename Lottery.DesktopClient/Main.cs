using Lottery.DesktopClient.Card;
using Lottery.DesktopClient.Services;
using Lottery.Lib;
using Lottery.Lib.Configuration;
using Lottery.Lib.Core;
using Lottery.Lib.DependencyRegister;
using Lottery.Lib.Logging;
using Lottery.Lib.Players;
using Lottery.Lib.Tickets;
using Lottery.Lib.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Lottery.DesktopClient
{
    public partial class Main : Form
    {        
        LotteryCampaign _campaign;
        public Main()
        {
            InitializeComponent();
            Load += (s, e) => Init();
        }

        #region Initialization

        void Init()
        {
            flpBy.DoubleBuffered(true);
            flpTickets.DoubleBuffered(true);

            InitUIState();
            CreateServiceContainer();
            WireEvents();
        }
        void InitUIState()
        {
            flpBy.Controls.Clear();
            flpTickets.Controls.Clear();
            pnlDashboard.Visible = false;

            tbPlayer1ID.Text = string.Empty;
            tbPlayer1Name.Text = string.Empty;
            linkStartCampaign.Enabled = true;
            linkResetLotteryGame.Enabled = false;
            linkGeneratePlayers.Enabled = false;
            linkDrawWinners.Enabled = false;
        }
        void CreateServiceContainer()
        {
            DependencyContainer.Instance.Release();
            DependencyContainer.Instance.Initialize(services =>
            {
                services.RegisterAllDependencies();
                services.AddTransient<ILogger>(services => new RtbLogger(rtbLog));
            });           
        }
        void WireEvents()
        {
            linkHrMediator.Click += (s, e) => CopyHRMediatorEmail();
            linkClearLog.Click += (s, e) => ClearLog();
            linkResetLotteryGame.Click += (s, e) => ResetLotteryGame();
            linkStartCampaign.Click += (s, e) => StartCampaign();
            tbPlayer1TicketsCount.MouseWheel += (s, e) => HandleMouseWheelPlayer1TicketsCount(e);
            linkIncreaseTicketCount.Click += (s, e) => IncreaseTicketCount();
            linkDecreaseTicketCount.Click += (s, e) => DecreaseTicketCount();
            linkGeneratePlayers.Click += (s, e) => GeneratePlayers();
            linkDrawWinners.Click += (s, e) => DrawWinners();
            linkProjectByTier.Click += (s, e) => CreateProjectionByTier();
            linkProjectByPlayer.Click += (s, e) => CreateProjectionByPlayer();
        }

        #endregion

        #region Handlers

        void ClearLog() => rtbLog.Clear();
        void ResetLotteryGame()
        {            
            _campaign = null;

            InitUIState();
            CreateServiceContainer();
        }
        void StartCampaign()
        {
            tabMainResults.Visible = true;
            _campaign = Get<LotteryCampaign>();
            linkStartCampaign.Enabled = false;
            pnlPlayer1Props.Enabled = true;
            linkResetLotteryGame.Enabled = true;
            linkGeneratePlayers.Enabled = true;
        }
        void HandleMouseWheelPlayer1TicketsCount(MouseEventArgs e)
        {
            var config = Get<Config>();
            int currentCount = int.Parse(tbPlayer1TicketsCount.Text);

            if (e.Delta > 0)
            {
                if (currentCount < config.Player.MaxTicketsCount)
                {
                    currentCount++;
                }
            }
            else
            {
                if (currentCount > config.Player.MinTicketsCount)
                {
                    currentCount--;
                }
            }

            tbPlayer1TicketsCount.Text = currentCount.ToString();
        }
        void IncreaseTicketCount()
        {
            var config = Get<Config>();
            int currentCount = int.Parse(tbPlayer1TicketsCount.Text);
            if (currentCount < config.Player.MaxTicketsCount)
            {
                currentCount++;
            }
            tbPlayer1TicketsCount.Text = currentCount.ToString();
        }
        void DecreaseTicketCount()
        {
            var config = Get<Config>();
            int currentCount = int.Parse(tbPlayer1TicketsCount.Text);
            if (currentCount > config.Player.MinTicketsCount)
            {
                currentCount--;
            }
            tbPlayer1TicketsCount.Text = currentCount.ToString();

        }
        void GeneratePlayers()
        {
            tabMainResults.Visible = true;

            var config = Get<Config>();            
            var playerGenerator = Get<PlayerGenerator>();
            var randomizer = Get<IRangeRandomizer>();

            int player1TicketsCount = tbPlayer1TicketsCount.Text.As<int>();

            var player1Descriptor = PlayerGeneratorDescriptor
                            .New
                            .WithType(PlayerType.Human)
                            .WithTicketCount(player1TicketsCount);

            var playerCPUDescriptor = PlayerGeneratorDescriptor
                                        .New
                                        .WithType(PlayerType.CPU)
                                        .Randomize(true);

            int cpuPlayersCount = randomizer.GetRandomInRange
            (
                config.Lottery.MinPlayersCount,
                config.Lottery.MaxPlayersCount - 1
            );

            var player1 = playerGenerator.GeneratePlayers(player1Descriptor).First();
            var cpuPlayers = playerGenerator.GeneratePlayers(playerCPUDescriptor, cpuPlayersCount);

            //Creating campaign
            _campaign.RegisterPlayers(player1);
            _campaign.RegisterPlayers(cpuPlayers);

            tbPlayer1ID.Text = player1.Id.ToString();
            tbPlayer1Name.Text = player1.Name;
            linkGeneratePlayers.Enabled = false;
            linkDrawWinners.Enabled = true;
        }
        void DrawWinners()
        {
            _campaign.DrawWinners();
            _campaign.LogResults();

            linkGeneratePlayers.Enabled = false;
            linkDrawWinners.Enabled = false;

            statusHouseProfit.Text = _campaign.WinningsResult.HouseProfit.ToString();
            statusTicketsCount.Text = _campaign.WinningsResult.TotalTicketsCount.ToString();
            statusTotalRevenue.Text = _campaign.WinningsResult.TotalRevenue.ToString();
            statusTotalWinners.Text = _campaign.WinningsResult.AllWinners.Count.ToString();
            statusPrizePayments.Text = _campaign.WinningsResult.TotalPrize.ToString();
            pnlDashboard.Visible = true;

        }
        void CreateProjectionByTier()
        {
            flpBy.Controls.Clear();
            flpTickets.Controls.Clear();

            var byTierProjection = _campaign.WinningsResult.AllWinners.Project(t => t.WinType);
            foreach (var pair in byTierProjection)
            {
                string tier = pair.Key.AsString();
                List<WinningTicket> ticketList = pair.Value;
                string txt = $"{tier}\nTICKETS : {ticketList.Count}";

                Control card = CardBuilder
                                    .New
                                    .WithText(txt)
                                    .WithClickHandler(arg =>
                                    {
                                        titleTickets.Text = ($"{tier} TICKETS").ToUpper();
                                        LoadTickets(ticketList);
                                    })
                                    .Build()
                                    .Card;

                flpBy.Controls.Add(card);
            }
        }
        void CreateProjectionByPlayer()
        {
            flpBy.Controls.Clear();
            flpTickets.Controls.Clear();

            var byTierProjection = _campaign.WinningsResult.AllWinners.Project(t => t.PlayerId);

            foreach (var pair in byTierProjection)
            {
                int playerId = pair.Key;
                List<WinningTicket> ticketList = pair.Value;
                decimal playerProfit = ticketList.Sum(t => t.WinningPrize);
                string txt = $"Player {playerId}\nTICKETS COUNT: {ticketList.Count}\nPLAYER PROFIT : ${playerProfit}";

                Control card = CardBuilder
                                    .New
                                    .WithText(txt)
                                    .WithClickHandler(arg =>
                                    {
                                        titleTickets.Text = ($"Player {playerId} TICKETS").ToUpper();
                                        LoadTickets(ticketList);
                                    })
                                    .Build()
                                    .Card;

                flpBy.Controls.Add(card);
            }
            flpBy.AddEmptySpace();
        }        
        void LoadTickets(List<WinningTicket> tickets)
        {
            flpTickets.Controls.Clear();

            foreach (var ticket in tickets)
            {
                string txt = $"{ticket.Number}\nPlayer {ticket.PlayerId}\n{ticket.WinType.AsString()}\nPRIZE : ${ticket.WinningPrize}";

                Control card = CardBuilder
                                    .New
                                    .WithText(txt)
                                    .Build()
                                    .Card;

                flpTickets.Controls.Add(card);
            }
        }

        #endregion

        #region Help methods

        void CopyHRMediatorEmail() => Clipboard.SetText(linkHrMediator.Text);
        T Get<T>() => DependencyContainer.Get<T>();

        #endregion
    }
}
