using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.External;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3
{
    public class CommandHandler : ICommandHandler
    {
        private DiscordSocketClient _client;
        private readonly CommandService _service;
        private readonly IServiceProvider _provider;

        public CommandHandler(CommandService service, IServiceProvider provider)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _service.AddModulesAsync(typeof(HelpService).Assembly, provider);
        }

        public Task Initialize(DiscordSocketClient client)
        {
            _client = client;
            _client.MessageReceived += HandleCommandAsync;
            return Task.CompletedTask;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            if (!(s is SocketUserMessage msg) || msg.Author.IsBot || msg.Channel is SocketDMChannel) return;
            var prefix = "dest!";
            var context = new CommandContextWithPrefix(_client, msg, prefix);
            var argPos = 0;
            if (!(msg.HasStringPrefix(prefix, ref argPos) ||
                msg.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;
            await s.Channel.TriggerTypingAsync();
            var result = await _service.ExecuteAsync(context, argPos, _provider);
            if (!result.IsSuccess)
            {
                await context.Channel.SendMessageAsync("Incorrect usage of the command");
                // TODO: Add something
            }
        }
    }
}