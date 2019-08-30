using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.Interface;
using System;

namespace DestDiscordBotV3.Service.Internal
{
    public class MusicFactory : IMusicFactory
    {
        public Music Create(ulong id, string query) =>
            new Music
            {
                Id = Guid.NewGuid(),
                GuildId = id,
                Query = query,
                Requested = DateTime.UtcNow
            };
    }
}