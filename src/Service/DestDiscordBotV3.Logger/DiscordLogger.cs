using Discord;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Logger
{
    public class DiscordLogger : IDiscordLogger
    {
        private readonly ILogging _logger;

        public DiscordLogger(ILogging logger)
        {
            _logger = logger;
        }

        public Task Log(LogMessage logMsg)
        {
            _logger.Log(logMsg.Message);
            return Task.CompletedTask;
        }
    }
}