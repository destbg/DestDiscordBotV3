using DestDiscordBotV3.Model;

namespace DestDiscordBotV3.Service.Interface
{
    public interface IMusicFactory
    {
        Music Create(ulong id, string query);
    }
}