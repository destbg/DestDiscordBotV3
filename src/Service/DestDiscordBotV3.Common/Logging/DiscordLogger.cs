namespace DestDiscordBotV3.Common.Logging
{
    using Discord;
    using System.Threading.Tasks;

    public class DiscordLogger : IDiscordLogger
    {
        private readonly ILogger _logger;

        public DiscordLogger(ILogger logger)
        {
            _logger = logger;
        }

        public Task Log(LogMessage msg)
        {
            if (msg.Exception is null)
                _logger.Log($"{msg.Source}: {msg.Message}");
            else _logger.Log($"{msg.Message}: {msg.Exception}");
            return Task.CompletedTask;
        }
    }
}