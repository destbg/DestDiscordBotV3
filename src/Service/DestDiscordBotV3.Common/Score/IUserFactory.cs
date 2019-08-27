using DestDiscordBotV3.Model;

namespace DestDiscordBotV3.Common.Score
{
    public interface IUserFactory
    {
        AppUser Create(ulong userId);
    }
}