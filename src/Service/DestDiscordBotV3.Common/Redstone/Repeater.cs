namespace DestDiscordBotV3.Common.Redstone
{
    using System;
    using System.Threading.Tasks;
    using System.Timers;

    public class Repeater : IRepeater
    {
        private readonly IGiveawayObserver _giveaway;
        private readonly IReminderObserver _reminder;
        private readonly IMusicObserver _music;
        private readonly Timer _minute;
        private readonly Timer _tenSeconds;

        public Repeater(IGiveawayObserver giveaway, IReminderObserver reminder, IMusicObserver music)
        {
            _giveaway = giveaway ?? throw new ArgumentNullException(nameof(giveaway));
            _reminder = reminder ?? throw new ArgumentNullException(nameof(reminder));
            _music = music ?? throw new ArgumentNullException(nameof(music));
            _tenSeconds = new Timer
            {
                Enabled = true,
                AutoReset = true,
                Interval = 10000
            };
            _minute = new Timer
            {
                Enabled = true,
                AutoReset = true,
                Interval = 60000
            };
        }

        public Task InitializeAsync()
        {
            _tenSeconds.Elapsed += TenSeoncdsPassed;
            _minute.Elapsed += MinutePassed;
            return Task.CompletedTask;
        }

        private void TenSeoncdsPassed(object sender, ElapsedEventArgs e) =>
            _music.TenSecondsPassedAsync().GetAwaiter().GetResult();

        private void MinutePassed(object sender, ElapsedEventArgs e)
        {
            _giveaway.MinutePassedAsync().GetAwaiter().GetResult();
            _reminder.MinutePassedAsync().GetAwaiter().GetResult();
        }
    }
}