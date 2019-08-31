using System.Threading.Tasks;
using DestDiscordBotV3.Model;
using Discord.WebSocket;

namespace DestDiscordBotV3.Common.NewUser
{
    public interface INewUserHandler
    {
        Task<bool> NewUserMessage(CommandContextWithPrefix msg);
        Task RoleUpdated(SocketRole roleBefore, SocketRole roleNow);
        Task UserJoined(SocketGuildUser user);
    }
}