namespace DestDiscordBotV3.Service.Internal
{
    using Model;
    using Service.Interface;

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