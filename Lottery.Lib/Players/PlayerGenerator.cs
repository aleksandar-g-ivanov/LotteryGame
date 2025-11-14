using Lottery.Lib.Configuration;
using Lottery.Lib.Factories;
using Lottery.Lib.Logging;
using Lottery.Lib.Tickets;
using Lottery.Lib.Utils;

namespace Lottery.Lib.Players
{
    public class PlayerGenerator
    {
        readonly Config _config;
        readonly IRangeRandomizer _rnd;
        readonly FactoryRegister _factoryRegister;
        readonly ILogger _logger;
        public PlayerGenerator(Config config, IRangeRandomizer rnd, FactoryRegister factoryRegister, ILogger logger)
        {
            _config = config;
            _rnd = rnd;
            _factoryRegister = factoryRegister;
            _logger = logger;
        }

        public Player[] GeneratePlayers(PlayerGeneratorDescriptor descriptor, int count = 1)
        {
            List<Player> players = new();
            for (int i = 1; i <= count; i++)
            {
                var player = CreatePlayer(descriptor);
                players.Add(player);
            }

            return players.ToArray();
        }

        Player CreatePlayer(PlayerGeneratorDescriptor descriptor)
        {
            Player player = _factoryRegister.Create<Player>();
            player.PlayerType = descriptor.Type;
            int ticketCount = descriptor.TicketCount;
            if (descriptor.IsTicketCountRandom)
            {
                ticketCount = _rnd.GetRandomInRange(_config.Player.MinTicketsCount, _config.Player.MaxTicketsCount);
            }
            Ticket[] tickets = _factoryRegister.GetFactory<Ticket>().CreateTickets(ticketCount);
            player.PurchaseTickets(tickets);
            _logger.Info($"Created {player.PlayerType.AsString(),-6} {player.Name, -10} with {player.TicketsCount, 3} tickets.", true);
            return player;
        }
    }
}
