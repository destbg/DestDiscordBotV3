namespace DestDiscordBotV3.Common.Redstone
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IRepeater" />
    /// </summary>
    public interface IRepeater
    {
        /// <summary>
        /// Initialize the <see cref="Repeater"/> class
        /// </summary>
        Task InitializeAsync();
    }
}