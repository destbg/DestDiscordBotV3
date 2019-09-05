namespace DestDiscordBotV3.Service.Internal
{
    using Model;
    using Service.Interface;
    using System;

    public class ReminderFactory : IReminderFactory
    {
        public Reminder Create(ulong channelId, ulong userId, string message, DateTime endTime) =>
            new Reminder
            {
                Id = Guid.NewGuid(),
                ChannelId = channelId,
                UserId = userId,
                Message = message,
                EndTime = endTime
            };
    }
}