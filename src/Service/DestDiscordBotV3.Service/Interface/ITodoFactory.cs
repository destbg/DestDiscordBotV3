using DestDiscordBotV3.Model;

namespace DestDiscordBotV3.Service.Interface
{
    public interface ITodoFactory
    {
        Todo Create(ulong userId, string msg);
    }
}