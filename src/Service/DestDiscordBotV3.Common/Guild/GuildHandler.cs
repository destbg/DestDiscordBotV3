namespace DestDiscordBotV3.Common.Guild
{
    using Data;
    using Model;
    using System;
    using System.Threading.Tasks;

    public class GuildHandler : IGuildHandler
    {
        private readonly IRepository<AppGuild> _guild;
        private readonly IGuildFactory _guildFactory;

        public GuildHandler(IRepository<AppGuild> guild, IGuildFactory guildFactory)
        {
            _guild = guild ?? throw new ArgumentNullException(nameof(guild));
            _guildFactory = guildFactory ?? throw new ArgumentNullException(nameof(guildFactory));
        }

        public async Task<AppGuild> GetGuild(ulong id)
        {
            AppGuild guild;
            try
            {
                guild = await _guild.GetById(id);
            }
            catch
            {
                guild = null;
            }
            if (guild == null)
            {
                var appGuild = _guildFactory.Create(id);
                await _guild.Create(appGuild);
                return appGuild;
            }
            return guild;
        }
    }
}