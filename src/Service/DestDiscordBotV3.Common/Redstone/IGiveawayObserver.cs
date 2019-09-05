namespace DestDiscordBotV3.Common.Redstone
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IGiveawayObserver" />
    /// </summary>
    public interface IGiveawayObserver
    {
        /// <summary>
        /// Call this method every minute
        /// </summary>
        Task MinutePassedAsync();
    }
}