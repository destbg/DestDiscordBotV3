using static System.Console;

namespace DestDiscordBotV3.Logger
{
    public class Logging : ILogging
    {
        public void Log(string message) =>
            WriteLine(message);
    }
}