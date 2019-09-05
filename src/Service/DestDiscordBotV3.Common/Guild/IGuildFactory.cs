namespace DestDiscordBotV3.Common.Guild
{
    using DestDiscordBotV3.Model;

    /// <summary>
    /// Defines the <see cref="IGuildFactory" />
    /// </summary>
    public interface IGuildFactory
    {
        /// <summary>
        /// Create an <see cref="AppGuild"/> model from the specified arguments
        /// </summary>
        AppGuild Create(ulong id);
    }
}