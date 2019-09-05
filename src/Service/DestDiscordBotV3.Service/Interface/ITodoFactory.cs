namespace DestDiscordBotV3.Service.Interface
{
    using Model;

    /// <summary>
    /// Defines the <see cref="ITodoFactory" />
    /// </summary>
    public interface ITodoFactory
    {
        /// <summary>
        /// Create an <see cref="Todo"/> class from the specified arguments
        /// </summary>
        Todo Create(ulong userId, string msg);
    }
}