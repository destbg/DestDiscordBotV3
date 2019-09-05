namespace DestDiscordBotV3.Service.Interface
{
    using Model;

    /// <summary>
    /// Defines the <see cref="ICustomCommandFactory" />
    /// </summary>
    public interface ICustomCommandFactory
    {
        /// <summary>
        /// Create an <see cref="CustomCommand"/> class from the specified arguments
        /// </summary>
        CustomCommand Create(string command, string message);
    }
}