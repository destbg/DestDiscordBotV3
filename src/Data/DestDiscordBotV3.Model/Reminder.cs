namespace DestDiscordBotV3.Model
{
    using System;

    public class Reminder
    {
        public Guid Id { get; set; }
        public ulong ChannelId { get; set; }
        public ulong UserId { get; set; }
        public string Message { get; set; }
        public DateTime EndTime { get; set; }
    }
}