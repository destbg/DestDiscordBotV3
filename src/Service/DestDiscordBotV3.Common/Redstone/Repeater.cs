using System;
using System.Timers;

namespace DestDiscordBotV3.Common.Redstone
{
    public class Repeater : IRepeater
    {
        private readonly IGiveawayHandler _giveaway;
        private readonly Timer _minute;

        public Repeater(IGiveawayHandler giveaway)
        {
            _giveaway = giveaway ?? throw new ArgumentNullException(nameof(giveaway));
            _minute = new Timer
            {
                Enabled = true,
                AutoReset = true,
                Interval = 60000
            };
            _minute.Elapsed += MinutePassed;
        }

        private void MinutePassed(object sender, ElapsedEventArgs e) =>
            _giveaway.MinutePassedAsync().GetAwaiter().GetResult();
    }
}
