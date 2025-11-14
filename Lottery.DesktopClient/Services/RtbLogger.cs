using Lottery.Lib.Logging;

namespace Lottery.DesktopClient.Services
{
    public class RtbLogger : ILogger
    {
        RichTextBox _rtb;
        public RtbLogger(RichTextBox rtb)
        {
            _rtb = rtb;
        }

        public void Info(string message, bool newLine = false)
        {
            _rtb.AppendText(message);
            if (newLine)
            {
                _rtb.AppendText("\n");
            }
        }
        public void Error(string message, string details = null)
        {
            _rtb.SelectionColor = Color.DarkRed;
            string formattedMessage = $"ERROR : {message}";
            if (details != null)
            {
                formattedMessage = $"{formattedMessage}\nDETAILS : {details}\n";
            }
            _rtb.AppendText(formattedMessage);
            _rtb.SelectionColor = _rtb.ForeColor; 
        }
    }
}
