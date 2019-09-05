namespace DestDiscordBotV3.Common.Logging
{
    using Discord;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IDiscordLogger" />
    /// </summary>
    public interface IDiscordLogger
    {
        /// <summary>
        /// Log <see cref="LogMessage"/> message into the console
        /// </summary>
        Task Log(LogMessage logMsg);
    }
}