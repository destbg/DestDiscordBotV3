namespace DestDiscordBotV3.Common.Guild
{
    using Model;

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