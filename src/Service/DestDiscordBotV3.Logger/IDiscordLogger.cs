using Discord;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Logger
{
    public interface IDiscordLogger
    {
        Task Log(LogMessage logMsg);
    }
}