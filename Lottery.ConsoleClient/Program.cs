using Lottery.Lib.Configuration;
using Lottery.Lib.Core;
using Lottery.Lib.DependencyRegister;
using Lottery.Lib.Logging;
using Lottery.Lib.Players;
using Lottery.Lib.Utils;
using Microsoft.Extensions.DependencyInjection;

void Run()
{
    // 1 . Initialize dependency container
    DependencyContainer.Create
    (
        services => services
                        .AddTransient<ILogger, ConsoleLogger>()      
                        .RegisterAllDependencies()
    );
    
    var config = Get<Config>();
    var randomizer = Get<IRangeRandomizer>();
    var playerGenerator = Get<PlayerGenerator>();
    var campaign = Get<LotteryCampaign>();
    
    int player1TicketCount = ReadInt
    (
        "How many tickets do you want to buy, Player 1 : ",
        "Not valid integer, try again"
    );

    var player1Descriptor = PlayerGeneratorDescriptor
                                .New
                                .WithType(PlayerType.Human)
                                .WithTicketCount(player1TicketCount);

    var playerCPUDescriptor = PlayerGeneratorDescriptor
                                .New
                                .WithType(PlayerType.CPU)
                                .Randomize(true);

    int cpuPlayersCount = randomizer.GetRandomInRange
    (
        config.Lottery.MinPlayersCount,
        config.Lottery.MaxPlayersCount - 1
    );

    var player1 = playerGenerator.GeneratePlayers(player1Descriptor);
    var cpuPlayers = playerGenerator.GeneratePlayers(playerCPUDescriptor, cpuPlayersCount);    

    // 3 . Register players
    campaign.RegisterPlayers(player1);
    campaign.RegisterPlayers(cpuPlayers);

    // 4 . Pick winners
    campaign.DrawWinners();

    // 5 . Print results
    campaign.LogResults();    
   
    Console.ReadLine();
}

int ReadInt(string msg, string errorMsg)
{
    while (true)
    {
        Console.Write(msg);
        string input = Console.ReadLine();
        if (int.TryParse(input, out int number))
        {
            return number;
        }
        Console.WriteLine(errorMsg);
    }
}

T Get<T>() => DependencyContainer.Get<T>();

Run();





