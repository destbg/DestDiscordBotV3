using System.Collections.Generic;

namespace DestDiscordBotV3.Model
{
    public class AppGuild
    {
        public AppGuild()
        {
            CustomCommands = new HashSet<CustomCommand>();
        }

        public ulong Id { get; set; }
        public string Prefix { get; set; }
        public ICollection<CustomCommand> CustomCommands { get; set; }
    }
}