using System;

namespace DestDiscordBotV3.Model
{
    public class GuildUser
    {
        public Guid Id { get; set; }
        public ulong UserId { get; set; }
        public ulong GuildId { get; set; }
        public ulong Points { get; set; }
    }
}
