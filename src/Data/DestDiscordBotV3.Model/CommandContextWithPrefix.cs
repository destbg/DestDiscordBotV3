using Discord.Commands;
using Discord.WebSocket;

namespace DestDiscordBotV3.Model
{
    public class CommandContextWithPrefix : SocketCommandContext, ICommandContext
    {
        public string Prefix { get; set; }

        public CommandContextWithPrefix(DiscordSocketClient client, SocketUserMessage msg, string prefix) : base(client, msg)
        {
            Prefix = prefix;
        }
    }
}