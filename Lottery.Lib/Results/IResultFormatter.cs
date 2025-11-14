namespace Lottery.Lib.Results
{
    public interface IResultFormatter
    {
        string Format(WinningTicketsResult wtr);
    }
}
