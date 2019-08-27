using System;

namespace DestDiscordBotV3.Model
{
    public class AppUser
    {
        public ulong Id { get; set; }
        public ulong Points { get; set; }
        public DateTime LastMessage { get; set; }
    }
}
