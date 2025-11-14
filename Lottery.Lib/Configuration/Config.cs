namespace Lottery.Lib.Configuration
{
    public class Config
    {
        public virtual LotteryProps Lottery { get; set; } = new();
        public virtual PlayerProps Player { get; set; } = new();
        public virtual TicketProps Ticket { get; set; } = new();
        public virtual PrizeProps Prize { get; set; } = new();

        #region Inner types

        public class LotteryProps
        {
            public virtual int MinPlayersCount { get; set; } = 10;
            public virtual int MaxPlayersCount { get; set; } = 15;
        }
        public class PlayerProps
        {
            public virtual int StartingBallance { get; set; } = 10;
            public virtual int MinTicketsCount { get; set; } = 1;
            public virtual int MaxTicketsCount { get; set; } = 10;
        }
        public class TicketProps
        {
            public virtual int TicketPrice { get; set; } = 1;
            public virtual int MinTicketNumber { get; set; } = 100;
            public virtual int MaxTicketNumber { get; set; } = 999;
        }
        public class PrizeProps
        {
            public virtual GrandPrizeProps GrandPrize { get; set; } = new();
            public virtual Tier2Props Tier2 { get; set; } = new();
            public virtual Tier3Props Tier3 { get; set; } = new();

            public class GrandPrizeProps
            {
                public virtual int PercentsFromRevenue { get; set; } = 50;
                public virtual int WinnersCount { get; set; } = 1;
            }

            public class Tier2Props
            {
                public virtual int PercentsFromRevenue { get; set; } = 30;
                public virtual int PercentsWinningTickets { get; set; } = 10;
            }

            public class Tier3Props
            {
                public virtual int PercentsFromRevenue { get; set; } = 10;
                public virtual int PercentsWinningTickets { get; set; } = 20;
            }

        }

        #endregion
    }
}
