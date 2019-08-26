using System.Threading.Tasks;

namespace DestDiscordBotV3
{
    public interface IConnection
    {
        Task ConnectAsync(string token);
    }
}