namespace DestDiscordBotV3.Service.Internal
{
    using Model;
    using Service.Interface;
    using System;

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