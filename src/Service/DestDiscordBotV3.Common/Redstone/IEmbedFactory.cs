namespace DestDiscordBotV3.Common.Redstone
{
    using Discord;
    using System;

    /// <summary>
    /// Defines the <see cref="IEmbedFactory" />
    /// </summary>
    public interface IEmbedFactory
    {
        /// <summary>
        /// Create an <see cref="Embed"/> from the specified arguments
        /// </summary>
        Embed Create(string title, Color color, string description, string footer, DateTimeOffset endTime);
    }
}