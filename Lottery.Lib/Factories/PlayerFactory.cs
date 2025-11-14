using Lottery.Lib.Configuration;
using Lottery.Lib.Players;

namespace Lottery.Lib.Factories
{    
    public class PlayerFactory : IFactory<Player>
    {
        Config _config;
        int _currentPlayerNumber;        
        public PlayerFactory(Config config)
        {
            _config = config;   
            _currentPlayerNumber = 1;                     
        }

        public Player Create()
        {            
            int id = _currentPlayerNumber;
            string playerName = $"Player {id}";

            var player = new Player()
            {
                Id = id,
                Ballance = _config.Player.StartingBallance,
                Name = playerName,
            };

            _currentPlayerNumber++;

            return player;
        }        
    }
}
