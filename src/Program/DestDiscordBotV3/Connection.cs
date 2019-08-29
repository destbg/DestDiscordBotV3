using DestDiscordBotV3.Common.Logging;
using DestDiscordBotV3.Common.Redstone;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3
{
    public class Connection : IConnection
    {
        private readonly DiscordSocketClient _client;
        private readonly ICommandHandler _handler;
        private readonly IDiscordLogger _logger;
        private readonly IRepeater _repeater;

        public Connection(DiscordSocketClient client, ICommandHandler handler, IDiscordLogger logger, IRepeater repeater)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repeater = repeater ?? throw new ArgumentNullException(nameof(repeater));
        }

        public async Task ConnectAsync(string token)
        {
            _client.Log += _logger.Log;
            await _client.LoginAsync(Discord.TokenType.Bot, token);
            await _client.StartAsync();
            await _client.SetGameAsync("Try dest!help");
            await _handler.Initialize();
            await Task.Delay(-1);
        }
    }
}