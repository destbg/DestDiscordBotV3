namespace DestDiscordBotV3.Service.Interface
{
    using Model;

    /// <summary>
    /// Defines the <see cref="IMusicFactory" />
    /// </summary>
    public interface IMusicFactory
    {
        /// <summary>
        /// Create an <see cref="Music"/> class from the specified arguments
        /// </summary>
        Music Create(ulong id, string query);
    }
}