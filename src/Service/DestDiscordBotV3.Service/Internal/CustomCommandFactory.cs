using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.Interface;

namespace DestDiscordBotV3.Service.Internal
{
    public class CustomCommandFactory : ICustomCommandFactory
    {
        public CustomCommand Create(string command, string message) =>
            new CustomCommand
            {
                Command = command,
                Message = message
            };
    }
}
