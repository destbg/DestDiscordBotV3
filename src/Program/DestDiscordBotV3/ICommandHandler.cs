namespace DestDiscordBotV3
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ICommandHandler" />
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Initialize the <see cref="CommandHandler" />
        /// </summary>
        Task InitializeAsync();
    }
}