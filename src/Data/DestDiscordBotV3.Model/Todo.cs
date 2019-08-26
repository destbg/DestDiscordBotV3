using System;

namespace DestDiscordBotV3.Model
{
    public class Todo
    {
        public Guid Id { get; set; }
        public ulong UserId { get; set; }
        public string Msg { get; set; }
    }
}