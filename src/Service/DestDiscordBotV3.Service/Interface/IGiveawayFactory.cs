namespace DestDiscordBotV3.Service.Interface
{
    using Model;
    using System;

    /// <summary>
    /// Defines the <see cref="IGiveawayFactory" />
    /// </summary>
    public interface IGiveawayFactory
    {
        /// <summary>
        /// Create an <see cref="Giveaway"/> class from the specified arguments
        /// </summary>
        Giveaway Create(ulong channelId, ulong messageId, int winnerCount, string title, DateTime endTime);
    }
}