namespace DestDiscordBotV3.Model
{
    using System;

    public class Tag
    {
        public Guid Id { get; set; }
        public ulong UserId { get; set; }
        public string Name { get; set; }
        public string Msg { get; set; }
    }
}