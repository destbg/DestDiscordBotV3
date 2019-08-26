using System;

namespace DestDiscordBotV3.Model
{
    public class Report
    {
        public Guid Id { get; set; }
        public string User { get; set; }
        public string Guild { get; set; }
        public string Message { get; set; }
    }
}