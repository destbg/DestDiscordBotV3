using Discord;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Common.Logging
{
    public interface IDiscordLogger
    {
        Task Log(LogMessage logMsg);
    }
}