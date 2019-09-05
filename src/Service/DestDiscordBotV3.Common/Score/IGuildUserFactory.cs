namespace DestDiscordBotV3.Common.Score
{
    using Model;

    /// <summary>
    /// Defines the <see cref="IGuildUserFactory" />
    /// </summary>
    public interface IGuildUserFactory
    {
        /// <summary>
        /// Create an <see cref="GuildUser"/> class from the specified arguments
        /// </summary>
        GuildUser Create(ulong userId, ulong guildId, ulong points);
    }
}