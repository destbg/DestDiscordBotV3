using DestDiscordBotV3.Model;

namespace DestDiscordBotV3.Common.Guild
{
    public class GuildFactory : IGuildFactory
    {
        public AppGuild Create(ulong id) =>
            new AppGuild
            {
                Id = id,
                Prefix = "dest!"
            };
    }
}