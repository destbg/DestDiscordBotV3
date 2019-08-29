using DestDiscordBotV3.Service.Interface;
using Discord;
using System;

namespace DestDiscordBotV3.Service.Internal
{
    public class EmbedFactory : IEmbedFactory
    {
        public Embed Create(string title, Color color, string description, string footer, DateTimeOffset endTime) =>
            new EmbedBuilder
            {
                Title = title,
                Color = color,
                Description = description,
                Footer = new EmbedFooterBuilder
                {
                    Text = footer
                },
                Timestamp = endTime
            }.Build();
    }
}
