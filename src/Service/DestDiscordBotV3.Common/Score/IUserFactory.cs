namespace DestDiscordBotV3.Common.Score
{
    using DestDiscordBotV3.Model;

    /// <summary>
    /// Defines the <see cref="IUserFactory" />
    /// </summary>
    public interface IUserFactory
    {
        /// <summary>
        /// Create and <see cref="AppUser"/> class from the specified arguments
        /// </summary>
        AppUser Create(ulong userId);
    }
}