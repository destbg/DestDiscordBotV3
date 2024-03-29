﻿namespace DestDiscordBotV3.Common.Redstone
{
    using Discord;
    using System;

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