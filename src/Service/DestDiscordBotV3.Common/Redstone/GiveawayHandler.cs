using DestDiscordBotV3.Data;
using DestDiscordBotV3.Data.Extension;
using DestDiscordBotV3.Model;
using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Common.Redstone
{
    public class GiveawayHandler : IGiveawayHandler
    {
        private readonly IRepository<Giveaway> _giveaway;
        private readonly DiscordSocketClient _client;
        private readonly IEmbedFactory _embedFactory;

        public GiveawayHandler(IRepository<Giveaway> giveaway, DiscordSocketClient client, IEmbedFactory embedFactory)
        {
            _giveaway = giveaway ?? throw new ArgumentNullException(nameof(giveaway));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _embedFactory = embedFactory ?? throw new ArgumentNullException(nameof(embedFactory));
        }

        public async Task MinutePassedAsync() =>
            await _giveaway.GetAll().ForEach(async giveaway =>
            {
                if (!(_client.GetChannel(giveaway.ChannelId) is ITextChannel channel) ||
                    !(await channel.GetMessageAsync(giveaway.MessageId) is IUserMessage message))
                {
                    await _giveaway.Delete(giveaway.Id);
                    return;
                }
                var now = DateTime.UtcNow;
                if (now >= giveaway.EndTime)
                {
                    await DrawGiveawayWinner(giveaway, channel, message);
                    return;
                }
                var timeleft = giveaway.EndTime.Subtract(now);
                var embed = _embedFactory.Create(giveaway.Title,
                    Color.Blue,
                    $"React with :tada: to enter!\nTime remaining: {(timeleft.TotalDays >= 1 ? $"{timeleft.Days} days " : "")}{(timeleft.TotalHours >= 1 ? $"{timeleft.Hours} hours " : "")}{(timeleft.TotalMinutes >= 1 ? $"{timeleft.Minutes} minutes " : "")}{timeleft.Seconds} seconds",
                    "Ends at",
                    giveaway.EndTime);

                await message.ModifyAsync(f => f.Embed = embed);
            });

        private async Task DrawGiveawayWinner(Giveaway giveaway, ITextChannel channel, IUserMessage message)
        {
            await _giveaway.Delete(giveaway.Id);
            var winners = (await message.GetReactionUsersAsync(new Emoji("🎉"), int.MaxValue).ToList())
                .FirstOrDefault()
                .OrderBy(f => Guid.NewGuid())
                .Where(w => !w.IsBot)
                .Take(giveaway.WinnerCount)
                .Select(s => s.Mention);
            Embed embed;
            if (winners.Count() < giveaway.WinnerCount)
            {
                embed = _embedFactory.Create(giveaway.Title,
                    Color.DarkRed,
                    "Giveaway didn't get the required amount of people to join",
                    "Ended at",
                    giveaway.EndTime);
                await channel.SendMessageAsync("Giveaway didn't get the required amount of people to join.");
            }
            else
            {
                embed = _embedFactory.Create(giveaway.Title,
                    Color.DarkRed,
                    string.Join("\n", winners),
                    "Ended at",
                    giveaway.EndTime);
                await channel.SendMessageAsync($"Congratulations {string.Join(" ", winners)}! You won the **{giveaway.Title}**!");
            }
            await message.ModifyAsync(f =>
            {
                f.Content = ":tada: GIVEAWAY ENDED :tada:";
                f.Embed = embed;
            });
        }
    }
}
