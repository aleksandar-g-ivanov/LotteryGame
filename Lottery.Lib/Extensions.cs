using Lottery.Lib.Factories;
using Lottery.Lib.Players;
using Lottery.Lib.Prizing.Enums;
using Lottery.Lib.Tickets;
using Lottery.Lib.Utils;

namespace Lottery.Lib
{
    public static class Extensions
    {
        public static void CallDispose<Т>(this Т obj) where Т : class, IDisposable
        {
            if (obj is IDisposable disposable)
            {
                disposable.Dispose();                
            }
        }

        public static Ticket[] CreateTickets(this IFactory<Ticket> ticketFac, int ticketsCount)
        {
            List<Ticket> tickets = new();
            for (int i = 1; i <= ticketsCount; i++)
            {
                var newTicket = ticketFac.Create();
                tickets.Add(newTicket);
            }
            return tickets.ToArray();
        }

        public static int PickIndexInRange(this HashSet<int> cache, IRangeRandomizer rnd, int from, int to)
        {
            int number = 0;
            do
            {
                number = rnd.GetRandomInRange(from, to);
            }
            while (!cache.Add(number));

            return number;
        }
        public static List<int> PickCountIndexesInRange(this HashSet<int> cache, int count, IRangeRandomizer rnd, int from, int to)
        {
            List<int> indexes = new();
            while (indexes.Count < count && cache.Count < to)
            {
                int i = cache.PickIndexInRange(rnd, from, to);
                indexes.Add(i);
            }

            return indexes;
        }
        public static SortedDictionary<T, List<U>> Project<T, U>(this List<U> original, Func<U, T> keyMap)
        {
            SortedDictionary<T, List<U>> projection = new();

            foreach (U item in original)
            {
                T key = keyMap(item);
                if (!projection.ContainsKey(key))
                {
                    projection.Add(key, new List<U> { item });
                }
                else
                {
                    projection[key].Add(item);
                }
            }

            return projection;
        }
        public static string AsString(this WinType type) => type switch
        {
            WinType.None => "None",
            WinType.GrandPrize => "Grand Prize",
            WinType.SecondTier => "Second Tier",
            WinType.ThirdTier => "Third Tier",
            _ => "None"
        };

        public static string AsString(this PlayerType type) => type switch
        {
            PlayerType.Human => "Human",
            PlayerType.CPU => "CPU",            
            _ => "None"
        };
    }
}
