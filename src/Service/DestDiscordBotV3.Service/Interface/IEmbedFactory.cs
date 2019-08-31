using Discord;
using System;

namespace DestDiscordBotV3.Service.Interface
{
    public interface IEmbedFactory
    {
        Embed Create(string title, Color color, string description, string footer, DateTimeOffset endTime);
    }
}