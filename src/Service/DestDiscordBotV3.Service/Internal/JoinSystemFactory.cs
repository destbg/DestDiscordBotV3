using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.Interface;

namespace DestDiscordBotV3.Service.Internal
{
    public class JoinSystemFactory : IJoinSystemFactory
    {
        public JoinSystem Create(int rolesRequired, string doneMessage, ulong joinRole, ulong finishRole, params ulong[] roles) =>
            new JoinSystem
            {
                RolesRequired = rolesRequired,
                DoneMessage = doneMessage,
                JoinRole = joinRole,
                FinishRole = finishRole,
                Roles = roles
            };
    }
}
