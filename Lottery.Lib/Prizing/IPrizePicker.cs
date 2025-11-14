using Lottery.Lib.Tickets;

namespace Lottery.Lib.Core
{
    public interface IPrizePicker
    {
        decimal TotalRevenue { get; }
        int TicketsCount { get; }
        WinningTicket Prize1st();
        List<WinningTicket> Prize2nd();
        List<WinningTicket> Prize3rd();
    }
}
