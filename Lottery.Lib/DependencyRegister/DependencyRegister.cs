using Lottery.Lib.Factories;
using Microsoft.Extensions.DependencyInjection;
using Lottery.Lib.Core;
using Lottery.Lib.Tickets;
using Lottery.Lib.Results;
using Lottery.Lib.Utils;
using Lottery.Lib.Players;
using Lottery.Lib.Configuration;
using Lottery.Lib.Prizing;

namespace Lottery.Lib.DependencyRegister
{
    public static class DependencyContainerExtensions
    {
        public static void RegisterAllDependencies(this IServiceCollection services)
        {
            services
                .AddSingleton<Config>()
                .AddSingleton<ITicketPool, TicketPool>()
                .AddTransient<IRangeRandomizer, RangeRandomizer>()
                .AddTransient<IPrizePicker, PrizePicker>()
                .AddTransient<ITierCalculator, TierCalculator>()
                .AddTransient<IResultFormatter, LotteryResultFormatter>()
                .AddTransient<PlayerGenerator>()
                .AddTransient<LotteryCampaign>()
                .AddSingleton
                (
                    provider => FactoryRegister
                                    .Create(provider)
                                    .Register<Player, PlayerFactory>()
                                    .Register<Ticket, TicketFactory>()
                );
        }
    }
}
