using System;
using System.Timers;

namespace DestDiscordBotV3.Common.Redstone
{
    public class Repeater : IRepeater
    {
        private readonly INewUserChecker _newUser;
        private readonly IGiveawayHandler _giveaway;
        private readonly IReminderHandler _reminder;
        private readonly IMusicChecker _music;
        private readonly Timer _minute;
        private readonly Timer _tenMinute;
        private readonly Timer _tenSeconds;

        public Repeater(INewUserChecker newUser, IGiveawayHandler giveaway, IReminderHandler reminder, IMusicChecker music)
        {
            _newUser = newUser ?? throw new ArgumentNullException(nameof(newUser));
            _giveaway = giveaway ?? throw new ArgumentNullException(nameof(giveaway));
            _reminder = reminder ?? throw new ArgumentNullException(nameof(reminder));
            _music = music ?? throw new ArgumentNullException(nameof(music));
            _tenSeconds = new Timer
            {
                Enabled = true,
                AutoReset = true,
                Interval = 10000
            };
            _tenSeconds.Elapsed += TenSeoncdsPassed;
            _minute = new Timer
            {
                Enabled = true,
                AutoReset = true,
                Interval = 60000
            };
            _minute.Elapsed += MinutePassed;
            _tenMinute = new Timer
            {
                Enabled = true,
                AutoReset = true,
                Interval = 600000
            };
            _tenMinute.Elapsed += TenMinutePassed;
        }

        private void TenSeoncdsPassed(object sender, ElapsedEventArgs e) =>
            _music.TenSecondsPassedAsync().GetAwaiter().GetResult();

        private void MinutePassed(object sender, ElapsedEventArgs e)
        {
            _giveaway.MinutePassedAsync().GetAwaiter().GetResult();
            _reminder.MinutePassedAsync().GetAwaiter().GetResult();
        }

        private void TenMinutePassed(object sender, ElapsedEventArgs e) =>
            _newUser.CheckNewUsersAsync().GetAwaiter().GetResult();
    }
}
