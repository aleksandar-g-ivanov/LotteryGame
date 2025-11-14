using System.Text;

namespace Lottery.Lib.Results
{
    public class LotteryResultFormatter : IResultFormatter
    {
        public string Format(WinningTicketsResult wtr)
        {
            StringBuilder b = new();

            var byTier = wtr.AllWinners.Project(t => t.WinType);
            foreach (var tierValuePair in byTier)
            {
                b.Append($"* {tierValuePair.Key.AsString()} : ");
                var byPlayer = tierValuePair.Value.Project(t => t.PlayerId);

                foreach (var playerValuePair in byPlayer)
                {
                    string playerItem = $"Player {playerValuePair.Key}({playerValuePair.Value.Count}) ";
                    b.Append(playerItem);
                }
                b.AppendLine();
            }
            b.AppendLine($"House Profit is ${wtr.HouseProfit}");

            return b.ToString();
        }
    }
}
