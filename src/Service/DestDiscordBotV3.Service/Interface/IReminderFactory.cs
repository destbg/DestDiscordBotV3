using System;
using DestDiscordBotV3.Model;

namespace DestDiscordBotV3.Service.Interface
{
    public interface IReminderFactory
    {
        Reminder Create(ulong channelId, ulong userId, string message, DateTime endTime);
    }
}