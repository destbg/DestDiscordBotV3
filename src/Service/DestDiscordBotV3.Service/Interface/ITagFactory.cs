namespace DestDiscordBotV3.Service.Interface
{
    using Model;

    /// <summary>
    /// Defines the <see cref="ITagFactory" />
    /// </summary>
    public interface ITagFactory
    {
        /// <summary>
        /// Create an <see cref="Tag"/> class from the specified arguments
        /// </summary>
        Tag Create(ulong userId, string tag, string message);
    }
}