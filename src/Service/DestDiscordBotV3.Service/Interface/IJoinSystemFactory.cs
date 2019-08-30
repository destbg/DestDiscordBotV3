using DestDiscordBotV3.Model;

namespace DestDiscordBotV3.Service.Interface
{
    public interface IJoinSystemFactory
    {
        JoinSystem Create(int rolesRequied, string doneMessage, ulong joinRole, ulong finishRole, params ulong[] roles);
    }
}