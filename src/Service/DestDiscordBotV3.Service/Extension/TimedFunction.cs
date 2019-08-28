using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using Discord;
using Discord.Commands;
using System;
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

        public static async Task Remainder(ICommandContext context, int minutes)
        {
            var time = TimeSpan.FromMinutes(minutes);
            await Task.Delay((int)time.TotalMilliseconds);
            await context.Channel.SendMessageAsync($"{context.User.Mention} Times up!");
        }

        public static async Task RemoveMusic(IRepository<Music> music, Guid id, int seconds)
        {
            await Task.Delay(seconds * 1000);
            await music.Delete(id);
        }
    }
}