using System.Collections.Generic;

namespace DestDiscordBotV3.Model
{
    public class AppHelp
    {
        public string Id { get; set; }
        public HelpType HelpType { get; set; }
        public string Description { get; set; }
        public IReadOnlyList<string> Aliases { get; set; }
        public IReadOnlyList<string> Usages { get; set; }
        public IReadOnlyList<string> Examples { get; set; }
    }
}