namespace DestDiscordBotV3.Common.Redstone
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IMusicObserver" />
    /// </summary>
    public interface IMusicObserver
    {
        /// <summary>
        /// Call this method every 10 seconds
        /// </summary>
        Task TenSecondsPassedAsync();
    }
}