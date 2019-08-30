using System;
using System.Timers;

namespace DestDiscordBotV3.Common.Redstone
{
    public class Repeater : IRepeater
    {
        private readonly INewUserChecker _newUser;
        private readonly IGiveawayHandler _giveaway;
        private readonly Timer _minute;
        private readonly Timer _tenMinute;

        public Repeater(INewUserChecker newUser, IGiveawayHandler giveaway)
        {
            _newUser = newUser ?? throw new ArgumentNullException(nameof(newUser));
            _giveaway = giveaway ?? throw new ArgumentNullException(nameof(giveaway));
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

        private void MinutePassed(object sender, ElapsedEventArgs e) =>
            _giveaway.MinutePassedAsync().GetAwaiter().GetResult();

        private void TenMinutePassed(object sender, ElapsedEventArgs e) =>
            _newUser.CheckNewUsers().GetAwaiter().GetResult();
    }
}
