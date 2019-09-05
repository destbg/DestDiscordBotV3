namespace DestDiscordBotV3.Common.Redstone
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IReminderObserver" />
    /// </summary>
    public interface IReminderObserver
    {
        /// <summary>
        /// Call this method every minute
        /// </summary>
        Task MinutePassedAsync();
    }
}