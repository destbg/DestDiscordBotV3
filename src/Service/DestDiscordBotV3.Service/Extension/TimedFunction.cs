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
    }
}