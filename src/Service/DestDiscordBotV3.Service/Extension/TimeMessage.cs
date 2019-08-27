using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.Extension
{
    public static class TimeMessage
    {
        public static async Task Send(IUserMessage message, int seconds)
        {
            await Task.Delay(seconds * 1000);
            await message.DeleteAsync();
        }

        public static async Task Remainder(ICommandContext context, int minutes)
        {
            var time = TimeSpan.FromMinutes(minutes);
            await Task.Delay((int)time.TotalMilliseconds);
            await context.Channel.SendMessageAsync($"{context.User.Mention} Times up!");
        }
    }
}
