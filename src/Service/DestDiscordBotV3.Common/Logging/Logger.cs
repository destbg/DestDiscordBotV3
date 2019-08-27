using static System.Console;

namespace DestDiscordBotV3.Common.Logging
{
    public class Logger : ILogger
    {
        public void Log(string message) =>
            WriteLine(message);
    }
}