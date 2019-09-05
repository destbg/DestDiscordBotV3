namespace DestDiscordBotV3
{
    /// <summary>
    /// Defines the <see cref="IDInjection" />
    /// </summary>
    public interface IDInjection
    {
        /// <summary>
        /// Get a dependency
        /// </summary>
        T Resolve<T>();
    }
}