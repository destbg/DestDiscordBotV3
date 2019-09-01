using System.Threading.Tasks;
using DestDiscordBotV3.Model;

namespace DestDiscordBotV3.Common.Guild
{
    public interface IGuildHandler
    {
        Task<AppGuild> GetGuild(ulong id);
    }
}