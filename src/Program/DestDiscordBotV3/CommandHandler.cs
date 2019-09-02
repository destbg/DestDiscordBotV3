using DestDiscordBotV3.Common.Guild;
using DestDiscordBotV3.Common.Logging;
using DestDiscordBotV3.Common.Score;
using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.External;
using DestDiscordBotV3.Service.Interface;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DestDiscordBotV3
{
    public class CommandHandler : ICommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _service;
        private readonly IDiscordLogger _logger;
        private readonly IServiceProvider _provider;
        private readonly IPointsService _points;
        private readonly IGuildHandler _guild;

        public CommandHandler(DiscordSocketClient client, CommandService service, IDiscordLogger logger, IServiceProvider provider, IPointsService points, IGuildHandler guild)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _points = points ?? throw new ArgumentNullException(nameof(points));
            _guild = guild ?? throw new ArgumentNullException(nameof(guild));
            _service.AddModulesAsync(typeof(HelpService).Assembly, provider);
        }

        public async Task Initialize()
        {
            _service.Log += _logger.Log;
            _client.MessageReceived += HandleCommandAsync;

            // Initiate music handler
            var musicHandler = _provider.GetRequiredService<IMusicHandler>();
            await musicHandler.Initialize();
            _client.UserVoiceStateUpdated += musicHandler.UserVoiceStateUpdated;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            if (!(s is SocketUserMessage msg) || msg.Author.IsBot || msg.Channel is SocketDMChannel) return;

            var argPos = 0;
            var context = new CommandContextWithPrefix(_client, msg);
            var guild = await _guild.GetGuild(context.Guild.Id);
            context.Prefix = guild.Prefix;

            await _points.GivePoints(context.User.Id, context.Guild.Id).ConfigureAwait(false);

            if (!(msg.HasStringPrefix(guild.Prefix, ref argPos) ||
                msg.HasMentionPrefix(_client.CurrentUser, ref argPos)))
                return;

            await s.Channel.TriggerTypingAsync();
            var result = await _service.ExecuteAsync(context, argPos, _provider);

            if (!result.IsSuccess)
                await HandleResultAsync(result, context, guild);
        }

        private async Task HandleResultAsync(IResult result, CommandContextWithPrefix context, AppGuild guild)
        {
            switch (result.Error)
            {
                case CommandError.UnmetPrecondition:
                    await context.Channel.SendMessageAsync("You don't have the required permissions to use this command");
                    return;
                case CommandError.UnknownCommand:
                    if (await HandleCustomCommandAsync(context, guild))
                        return;
                    await context.Channel.SendMessageAsync($"Unknown command, for list of commands try using {context.Prefix}help");
                    return;
                case CommandError.ParseFailed:
                case CommandError.BadArgCount:
                    await context.Channel.SendMessageAsync($"Command wasn't used properly, try using {context.Prefix}help {GetCommandFromMessage(context.Message.Content, context.Prefix)}");
                    return;
                case CommandError.Unsuccessful:
                case CommandError.Exception:
                    await context.Channel.SendMessageAsync($"Command threw an exception, try reporting it using {context.Prefix}report <message>");
                    return;
                default:
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                    return;
            }
        }

        private async Task<bool> HandleCustomCommandAsync(CommandContextWithPrefix context, AppGuild guild)
        {
            if (!guild.CustomCommands.Any())
                return false;
            var command = guild.CustomCommands.FirstOrDefault(f => f.Command == GetCommandFromMessage(context.Message.Content, context.Prefix));
            if (command == null)
                return false;
            await context.Channel.SendMessageAsync(command.Message);
            return true;
        }

        private string GetCommandFromMessage(string message, string prefix) =>
            message.Split(' ')[0].Replace(prefix, "");
    }
}