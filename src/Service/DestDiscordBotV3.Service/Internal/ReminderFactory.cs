using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.Interface;
using System;

namespace DestDiscordBotV3.Service.Internal
{
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
