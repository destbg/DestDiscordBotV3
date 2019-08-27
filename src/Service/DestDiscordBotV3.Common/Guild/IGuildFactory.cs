using DestDiscordBotV3.Model;

namespace DestDiscordBotV3.Common.Guild
{
    public interface IGuildFactory
    {
        AppGuild Create(ulong id);
    }
}