namespace DestDiscordBotV3.Common.Guild
{
    using DestDiscordBotV3.Model;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IGuildHandler" />
    /// </summary>
    public interface IGuildHandler
    {
        /// <summary>
        /// Get an <see cref="AppGuild"/> model from the specified arguments
        /// </summary>
        Task<AppGuild> GetGuild(ulong id);
    }
}