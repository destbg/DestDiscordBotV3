namespace DestDiscordBotV3.Model
{
    using System;

    public class Music
    {
        public Guid Id { get; set; }
        public string Query { get; set; }
        public ulong GuildId { get; set; }
        public DateTime Requested { get; set; }
    }
}