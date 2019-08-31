using System;

namespace DestDiscordBotV3.Model
{
    public class Giveaway
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int WinnerCount { get; set; }
        public ulong ChannelId { get; set; }
        public ulong MessageId { get; set; }
        public DateTime EndTime { get; set; }
    }
}