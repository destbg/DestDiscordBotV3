namespace DestDiscordBotV3.Service.External
{
    using Data;
    using Data.Extension;
    using Discord;
    using Discord.Commands;
    using Model;
    using Service.Interface;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

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
        public async Task Create(int winnerCount, int time, string format, [Remainder] string title)
        {
            if (title.Length > 50)
            {
                await ReplyAsync("The length of the prize can be up to 50 characters");
                return;
            }
            var count = await _giveaway.GetAll().Where(w => w.ChannelId == Context.Channel.Id).CountDocumentsAsync();
            if (count > 5)
            {
                await ReplyAsync("You can only start up to 5 giveaways in a channel");
                return;
            }
            var (embed, endTime) = GetGiveawayMessage(time, format, title);
            if (endTime.Subtract(DateTime.UtcNow).TotalDays > 30)
            {
                await ReplyAsync("The giveaway can't be longer then 30 days");
                return;
            }
            if (embed is null)
            {
                await ReplyAsync("Time format is invalid");
                return;
            }
            var message = await ReplyAsync(":tada: GIVEAWAY :tada:", embed: embed);
            await message.AddReactionAsync(new Emoji("🎉"));

            await _giveaway.Create(_giveawayFactory.Create(Context.Channel.Id, message.Id, winnerCount, title, endTime));
        }

        [Command("edit")]
        public async Task Edit(ulong messageId, int winnerCount, int time, string format, [Remainder] string title)
        {
            if (title.Length > 50)
            {
                await ReplyAsync("The length of the prize can be up to 50 characters");
                return;
            }
            var giveaway = await _giveaway.GetByCondition(f => f.MessageId == messageId);
            if (giveaway is null)
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
            if (embed is null)
            {
                await ReplyAsync("Time format is invalid");
                return;
            }
            if (endTime.Subtract(DateTime.UtcNow).TotalDays > 30)
            {
                await ReplyAsync("The giveaway can't be longer then 30 days");
                return;
            }

            await message.ModifyAsync(f => f.Embed = embed);
            await message.AddReactionAsync(new Emoji("🎉"));

            giveaway.Title = title;
            giveaway.WinnerCount = winnerCount;
            giveaway.EndTime = endTime;
            await _giveaway.Update(giveaway, giveaway.Id);

            await ReplyAsync("Giveaway changed");
        }

        [Command("end")]
        public async Task End(ulong messageId)
        {
            var giveaway = await _giveaway.GetByCondition(f => f.MessageId == messageId);
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
        public async Task ReRoll(ulong messageId, int winnerCount)
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
                    return (null, default);
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