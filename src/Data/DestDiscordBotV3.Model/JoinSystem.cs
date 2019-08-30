using System.Collections.Generic;

namespace DestDiscordBotV3.Model
{
    public class JoinSystem
    {
        public ICollection<ulong> Roles { get; set; }
        public int RolesRequired { get; set; }
        public string DoneMessage { get; set; }
        public ulong JoinRole { get; set; }
        public ulong FinishRole { get; set; }
        public ulong? DefaultRole { get; set; }
    }
}
