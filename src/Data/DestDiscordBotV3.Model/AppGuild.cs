namespace DestDiscordBotV3.Model
{
    public class AppGuild
    {
        public ulong Id { get; set; }
        public string Prefix { get; set; }
        public JoinSystem JoinSystem { get; set; }
    }
}