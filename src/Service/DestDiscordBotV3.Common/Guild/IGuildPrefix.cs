using System.Threading.Tasks;

namespace DestDiscordBotV3.Common.Guild
{
    public interface IGuildPrefix
    {
        Task<string> GetGuildPrefix(ulong id);
    }
}