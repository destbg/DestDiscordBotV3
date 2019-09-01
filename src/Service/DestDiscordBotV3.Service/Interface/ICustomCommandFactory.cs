using DestDiscordBotV3.Model;

namespace DestDiscordBotV3.Service.Interface
{
    public interface ICustomCommandFactory
    {
        CustomCommand Create(string command, string message);
    }
}