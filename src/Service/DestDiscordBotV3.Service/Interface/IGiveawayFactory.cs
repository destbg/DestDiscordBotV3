using DestDiscordBotV3.Model;
using System;

namespace DestDiscordBotV3.Service.Interface
{
    public interface IGiveawayFactory
    {
        Giveaway Create(ulong channelId, ulong messageId, int winnerCount, string title, DateTime endTime);
    }
}