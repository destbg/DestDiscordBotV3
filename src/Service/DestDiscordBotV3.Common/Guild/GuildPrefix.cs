using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Common.Guild
{
    public class GuildPrefix : IGuildPrefix
    {
        private readonly IRepository<AppGuild> _guild;
        private readonly IGuildFactory _guildFactory;

        public GuildPrefix(IRepository<AppGuild> guild, IGuildFactory guildFactory)
        {
            _guild = guild ?? throw new ArgumentNullException(nameof(guild));
            _guildFactory = guildFactory ?? throw new ArgumentNullException(nameof(guildFactory));
        }

        public async Task<string> GetGuildPrefix(ulong id)
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
                await _guild.Create(_guildFactory.Create(id));
                return "dest!";
            }
            return guild.Prefix;
        }
    }
}
