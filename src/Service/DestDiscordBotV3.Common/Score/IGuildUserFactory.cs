using DestDiscordBotV3.Model;

namespace DestDiscordBotV3.Common.Score
{
    public interface IGuildUserFactory
    {
        GuildUser Create(ulong userId, ulong guildId, ulong points);
    }
}