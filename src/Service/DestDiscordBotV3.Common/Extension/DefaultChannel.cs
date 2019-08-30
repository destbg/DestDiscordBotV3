using Discord;
using Discord.WebSocket;
using System.Linq;

namespace DestDiscordBotV3.Common.Extension
{
    internal static class DefaultChannel
    {
        public static SocketTextChannel Get(SocketGuild guild) =>
            guild.Channels.FirstOrDefault(f => f.Id == guild.Id) is SocketTextChannel defaultChannel
                ? defaultChannel
                : guild.Channels.FirstOrDefault(f => f.Name == "general") is SocketTextChannel nameChannel
                ? nameChannel
                : guild.Channels.Where(c => c is SocketTextChannel channel &&
            channel.GetPermissionOverwrite(guild.EveryoneRole).GetValueOrDefault()
            .GetPerms()).OrderBy(a => a.Position).First() as SocketTextChannel;

        private static bool GetPerms(this OverwritePermissions value) =>
            value.SendMessages == PermValue.Inherit || value.SendMessages == PermValue.Allow
                && value.ViewChannel == PermValue.Inherit || value.ViewChannel == PermValue.Allow;
    }
}
