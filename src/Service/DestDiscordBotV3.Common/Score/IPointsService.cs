using System.Threading.Tasks;

namespace DestDiscordBotV3.Common.Score
{
    public interface IPointsService
    {
        Task GivePoints(ulong userId, ulong guildId);
    }
}