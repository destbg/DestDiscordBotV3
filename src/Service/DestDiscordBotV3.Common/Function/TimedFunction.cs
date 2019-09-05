namespace DestDiscordBotV3.Common.Function
{
    using Discord;
    using System.Threading.Tasks;

    /// <summary>
    /// <see cref="TimedFunction" /> class
    /// </summary>
    public static class TimedFunction
    {
        /// <summary>
        /// Send message that will get deleted after a couple of seconds
        /// </summary>
        public static async Task SendMessage(IUserMessage message, int seconds)
        {
            await Task.Delay(seconds * 1000);
            await message.DeleteAsync();
        }
    }
}