using DestDiscordBotV3.Common.Guild;
using DestDiscordBotV3.Common.NewUser;
using DestDiscordBotV3.Common.Score;
using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.External;
using DestDiscordBotV3.Service.Interface;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3
{
    public class CommandHandler : ICommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _service;
        private readonly INewUserHandler _newUser;
        private readonly IServiceProvider _provider;
        private readonly IPointsService _points;
        private readonly IGuildPrefix _prefix;

        public CommandHandler(DiscordSocketClient client, CommandService service, INewUserHandler newUser, IServiceProvider provider, IPointsService points, IGuildPrefix prefix)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _newUser = newUser ?? throw new ArgumentNullException(nameof(newUser));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _points = points ?? throw new ArgumentNullException(nameof(points));
            _prefix = prefix ?? throw new ArgumentNullException(nameof(prefix));
            _service.AddModulesAsync(typeof(HelpService).Assembly, provider);
        }

        public async Task Initialize()
        {
            _client.MessageReceived += HandleCommandAsync;

            // Initiate music handler
            var musicHandler = _provider.GetRequiredService<IMusicHandler>();
            await musicHandler.Initialize();
            _client.UserVoiceStateUpdated += musicHandler.UserVoiceStateUpdated;

            // Initiate new user handler
            _client.UserJoined += _newUser.UserJoined;
            _client.RoleUpdated += _newUser.RoleUpdated;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            if (!(s is SocketUserMessage msg) || msg.Author.IsBot || msg.Channel is SocketDMChannel) return;

            var argPos = 0;
            var context = new CommandContextWithPrefix(_client, msg);
            var prefix = await _prefix.GetGuildPrefix(context.Guild.Id);
            context.Prefix = prefix;

            await _points.GivePoints(context.User.Id, context.Guild.Id).ConfigureAwait(false);

            // New User Handler
            if (await _newUser.NewUserMessage(context))
                return;

            if (!(msg.HasStringPrefix(prefix, ref argPos) ||
                msg.HasMentionPrefix(_client.CurrentUser, ref argPos)))
                return;

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