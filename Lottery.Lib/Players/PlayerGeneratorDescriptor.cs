namespace Lottery.Lib.Players
{
    public class PlayerGeneratorDescriptor
    {
        public PlayerGeneratorDescriptor()
        {
            IsTicketCountRandom = false;
            TicketCount = 0;
        }
        public static PlayerGeneratorDescriptor New => new();
        public bool IsTicketCountRandom { get; set; }
        public int TicketCount { get; set; }
        public PlayerType Type { get; set; }

        public PlayerGeneratorDescriptor WithType(PlayerType type)
        {
            Type = type;
            return this;
        }
        public PlayerGeneratorDescriptor WithTicketCount(int ticketCount)
        {
            TicketCount = ticketCount;
            return this;
        }
        public PlayerGeneratorDescriptor Randomize(bool random)
        {
            IsTicketCountRandom = random;
            return this;
        }
    }
}
