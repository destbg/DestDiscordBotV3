namespace DestDiscordBotV3
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IConnection" />
    /// </summary>
    public interface IConnection
    {
        /// <summary>
        /// Connects the console to a discord bot with the specified token
        /// </summary>
        Task ConnectAsync(string token);
    }
}