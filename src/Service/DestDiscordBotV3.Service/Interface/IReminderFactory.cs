namespace DestDiscordBotV3.Service.Interface
{
    using Model;
    using System;

    /// <summary>
    /// Defines the <see cref="IReminderFactory" />
    /// </summary>
    public interface IReminderFactory
    {
        /// <summary>
        /// Create an <see cref="Reminder"/> class from the specified arguments
        /// </summary>
        Reminder Create(ulong channelId, ulong userId, string message, DateTime endTime);
    }
}