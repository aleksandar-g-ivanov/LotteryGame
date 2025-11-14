namespace Lottery.Lib.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Info(string message, bool newLine = true)
        {            
            Console.Write(message);
            if (newLine)
            {
                Console.WriteLine();
            }
        }

        public void Error(string message, string details = null)
        {
            Console.WriteLine(message);
            if (details != null)
            {
                Console.WriteLine(details);
            }
        }
    }
}
