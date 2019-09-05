namespace DestDiscordBotV3.Model
{
    using System;

    public class Report
    {
        public Guid Id { get; set; }
        public string User { get; set; }
        public string Guild { get; set; }
        public string Message { get; set; }
    }
}