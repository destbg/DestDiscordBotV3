namespace DestDiscordBotV3.Model
{
    using Discord.Commands;
    using Discord.WebSocket;

    /// <summary>
    /// Defines the <see cref="CommandContextWithPrefix" />
    /// </summary>
    public class CommandContextWithPrefix : SocketCommandContext, ICommandContext
    {
        /// <summary>
        /// Gets or sets the Prefix
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandContextWithPrefix"/> class.
        /// </summary>
        public CommandContextWithPrefix(DiscordSocketClient client, SocketUserMessage msg) : base(client, msg)
        {
        }
    }
}