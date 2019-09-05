namespace DestDiscordBotV3.Common.Logging
{
    /// <summary>
    /// Defines the <see cref="ILogger" />
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log message into the console
        /// </summary>
        void Log(string message);
    }
}