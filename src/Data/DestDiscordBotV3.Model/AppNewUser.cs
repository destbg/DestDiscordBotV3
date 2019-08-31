using System;

namespace DestDiscordBotV3.Model
{
    public class AppNewUser
    {
        public ulong Id { get; set; }
        public ulong GuildId { get; set; }
        public ulong UserId { get; set; }
        public ulong MessageId { get; set; }
        public DateTime LastMessage { get; set; }
    }
}
