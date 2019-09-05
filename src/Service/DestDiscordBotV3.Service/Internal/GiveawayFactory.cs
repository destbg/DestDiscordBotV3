namespace DestDiscordBotV3.Service.Internal
{
    using Model;
    using Service.Interface;
    using System;

    public class GiveawayFactory : IGiveawayFactory
    {
        public Giveaway Create(ulong channelId, ulong messageId, int winnerCount, string title, DateTime endTime) =>
            new Giveaway
            {
                Id = Guid.NewGuid(),
                WinnerCount = winnerCount,
                ChannelId = channelId,
                MessageId = messageId,
                Title = title,
                EndTime = endTime
            };
    }
}