using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.Interface;
using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    [Group("giveaway")]
    [RequireUserPermission(GuildPermission.ManageGuild)]
    public class GiveawayService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<Giveaway> _giveaway;
        private readonly IGiveawayFactory _giveawayFactory;
        private readonly IEmbedFactory _embedFactory;

        public GiveawayService(IRepository<Giveaway> giveaway, IGiveawayFactory giveawayFactory, IEmbedFactory embedFactory)
        {
            _giveaway = giveaway ?? throw new ArgumentNullException(nameof(giveaway));
            _giveawayFactory = giveawayFactory ?? throw new ArgumentNullException(nameof(giveawayFactory));
            _embedFactory = embedFactory ?? throw new ArgumentNullException(nameof(embedFactory));
        }

        [Command("create")]
        public async Task CreateAsync(int winnerCount, int time, string format, [Remainder] string title)
        {
            var (embed, endTime) = GetGiveawayMessage(time, format, title);
            var message = await ReplyAsync(":tada: GIVEAWAY :tada:", embed: embed);
            await message.AddReactionAsync(new Emoji("🎉"));

            await _giveaway.Create(_giveawayFactory.Create(Context.Channel.Id, message.Id, winnerCount, title, endTime));
        }

        [Command("edit")]
        public async Task EditAsync(ulong messageId, int winnerCount, int time, string format, [Remainder] string title)
        {
            var giveaway = await _giveaway.GetByExpression(f => f.MessageId == messageId);
            if (giveaway == null)
            {
                await ReplyAsync("Giveaway with that message id doesn't exist");
                return;
            }
            if (!(await Context.Channel.GetMessageAsync(giveaway.MessageId) is IUserMessage message))
            {
                await ReplyAsync("You must be in the same text channel as the giveaway");
                return;
            }
            if (message.Embeds.FirstOrDefault().Color == Color.DarkRed)
            {
                await ReplyAsync("That giveaway has already ended");
                return;
            }

            var (embed, endTime) = GetGiveawayMessage(time, format, title);

            await message.ModifyAsync(f => f.Embed = embed);
            await message.AddReactionAsync(new Emoji("🎉"));

            giveaway.Title = title;
            giveaway.WinnerCount = winnerCount;
            giveaway.EndTime = endTime;
            await _giveaway.Update(giveaway, giveaway.Id);

            await ReplyAsync("Giveaway changed");
        }

        [Command("end")]
        public async Task EndAsync(ulong messageId)
        {
            var giveaway = await _giveaway.GetByExpression(f => f.MessageId == messageId);
            if (giveaway == null)
            {
                await ReplyAsync("Giveaway with that message id doesn't exist");
                return;
            }
            if (!(await Context.Channel.GetMessageAsync(giveaway.MessageId) is IUserMessage message))
            {
                await ReplyAsync("You must be in the same text channel as the giveaway");
                return;
            }
            var lastEmbed = message.Embeds.FirstOrDefault();
            if (lastEmbed.Color == Color.DarkRed)
            {
                await ReplyAsync("That giveaway has already ended");
                return;
            }
            var embed = _embedFactory.Create(giveaway.Title,
                Color.DarkRed,
                "Giveaway was canceled",
                "Ended at",
                DateTime.UtcNow);

            await message.ModifyAsync(f => f.Embed = embed);
            await ReplyAsync("Giveaway was canceled");
        }

        [Command("reroll")]
        public async Task ReRollAsync(ulong messageId, int winnerCount)
        {
            if (!(await Context.Channel.GetMessageAsync(messageId) is IUserMessage message))
            {
                await ReplyAsync("You are not in the same channel as the giveaway or the giveaway doesn't exist");
                return;
            }
            var lastEmbed = message.Embeds.FirstOrDefault();
            if (lastEmbed.Color == Color.Blue)
            {
                await ReplyAsync("Giveaway is still not finished");
                return;
            }
            var winners = (await message.GetReactionUsersAsync(new Emoji("🎉"), int.MaxValue).ToList())
                .FirstOrDefault()
                .OrderBy(f => Guid.NewGuid())
                .Where(w => !w.IsBot)
                .Take(winnerCount)
                .Select(s => s.Mention);
            if (winners.Count() < winnerCount)
            {
                await Context.Channel.SendMessageAsync("Giveaway doesn't have the required amount of people.");
                return;
            }
            var embed = _embedFactory.Create(lastEmbed.Title,
                Color.DarkRed,
                string.Join("\n", winners),
                "Ended at",
                DateTime.UtcNow);
            await Context.Channel.SendMessageAsync($"Congratulations {string.Join(" ", winners)}! You won the **{lastEmbed.Title}**!");
            await message.ModifyAsync(f =>
            {
                f.Content = ":tada: GIVEAWAY ENDED :tada:";
                f.Embed = embed;
            });
        }

        private (Embed, DateTime) GetGiveawayMessage(int time, string format, string title)
        {
            DateTime endTime;
            switch (format.ToLower())
            {
                case "second":
                case "seconds":
                    endTime = DateTime.UtcNow.AddSeconds(time);
                    break;

                case "minute":
                case "minutes":
                    endTime = DateTime.UtcNow.AddMinutes(time);
                    break;

                case "hour":
                case "hours":
                    endTime = DateTime.UtcNow.AddHours(time);
                    break;

                case "day":
                case "days":
                    endTime = DateTime.UtcNow.AddDays(time);
                    break;

                default:
                    throw new Exception();
            }

            var timeleft = endTime.Subtract(DateTime.UtcNow);
            var embed = _embedFactory.Create(title,
                Color.Blue,
                $"React with :tada: to enter!\nTime remaining: {(timeleft.TotalDays >= 1 ? $"{timeleft.Days} days " : "")}{(timeleft.TotalHours >= 1 ? $"{timeleft.Hours} hours " : "")}{(timeleft.TotalMinutes >= 1 ? $"{timeleft.Minutes} minutes " : "")}{timeleft.Seconds} seconds",
                "Ends at",
                endTime);

            return (embed, endTime);
        }
    }
}