using DestDiscordBotV3.Data;
using DestDiscordBotV3.Data.Extension;
using DestDiscordBotV3.Model;
using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Common.Redstone
{
    public class ReminderObserver : IReminderObserver
    {
        private readonly IRepository<Reminder> _reminder;
        private readonly DiscordSocketClient _client;

        public ReminderObserver(IRepository<Reminder> reminder, DiscordSocketClient client)
        {
            _reminder = reminder ?? throw new ArgumentNullException(nameof(reminder));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task MinutePassedAsync()
        {
            var now = DateTime.UtcNow;
            await _reminder.GetAll().ForEach(async reminder =>
            {
                if (reminder.EndTime.Subtract(now).TotalMinutes > 0)
                    return;

                if (!(_client.GetChannel(reminder.ChannelId) is ITextChannel channel))
                {
                    await _reminder.Delete(reminder.Id);
                    return;
                }

                var user = await channel.GetUserAsync(reminder.UserId);
                if (user is null)
                {
                    await _reminder.Delete(reminder.Id);
                    return;
                }

                await channel.SendMessageAsync($"{user.Mention} {reminder.Message}");
                await _reminder.Delete(reminder.Id);
            });
        }
    }
}