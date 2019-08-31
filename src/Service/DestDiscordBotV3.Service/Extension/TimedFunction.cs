using Discord;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.Extension
{
    public static class TimedFunction
    {
        public static async Task SendMessage(IUserMessage message, int seconds)
        {
            await Task.Delay(seconds * 1000);
            await message.DeleteAsync();
        }
    }
}