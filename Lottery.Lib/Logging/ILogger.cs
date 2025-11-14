namespace Lottery.Lib.Logging
{
    public interface ILogger
    {
        void Info(string message, bool newLine = true);
        void Error(string message, string details = null);
    }
}
