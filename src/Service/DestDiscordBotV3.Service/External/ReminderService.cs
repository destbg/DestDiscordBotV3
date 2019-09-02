using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.Interface;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    [Group("reminder"), Alias("remindme")]
    public class ReminderService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<Reminder> _reminder;
        private readonly IReminderFactory _reminderFactory;

        public ReminderService(IRepository<Reminder> reminder, IReminderFactory reminderFactory)
        {
            _reminder = reminder ?? throw new ArgumentNullException(nameof(reminder));
            _reminderFactory = reminderFactory ?? throw new ArgumentNullException(nameof(reminderFactory));
        }

        [Command]
        public async Task Reminder(int minutes, [Remainder] string message)
        {
            await ReplyAsync($"Remainder set for after {minutes} minutes!");
            await _reminder.Create(_reminderFactory.Create(Context.Channel.Id, Context.User.Id, message, DateTime.UtcNow.AddMinutes(minutes)));
        }
    }
}