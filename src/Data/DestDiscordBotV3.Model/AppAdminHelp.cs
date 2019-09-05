namespace DestDiscordBotV3.Model
{
    using System.Collections.Generic;

    public class AppAdminHelp
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public IReadOnlyList<string> Aliases { get; set; }
        public IReadOnlyList<string> Usages { get; set; }
        public IReadOnlyList<string> Examples { get; set; }
    }
}