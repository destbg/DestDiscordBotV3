using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.Interface;

namespace DestDiscordBotV3.Service.Internal
{
    public class MusicFactory : IMusicFactory
    {
        public Music Create(ulong id, string query) =>
            new Music
            {
                Id = id,
                Query = query
            };
    }
}
