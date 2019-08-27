using Discord;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Common.Logging
{
    public class DiscordLogger : IDiscordLogger
    {
        private readonly ILogger _logger;

        public DiscordLogger(ILogger logger)
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