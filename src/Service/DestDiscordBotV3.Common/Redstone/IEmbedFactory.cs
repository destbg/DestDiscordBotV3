using System;
using Discord;

namespace DestDiscordBotV3.Common.Redstone
{
    public interface IEmbedFactory
    {
        Embed Create(string title, Color color, string description, string footer, DateTimeOffset endTime);
    }
}