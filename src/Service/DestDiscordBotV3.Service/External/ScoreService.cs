using DestDiscordBotV3.Data;
using DestDiscordBotV3.Data.Extension;
using DestDiscordBotV3.Model;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    [Group("scoreboard"), Alias("sB", "top")]
    public class ScoreService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<GuildUser> _user;

        public ScoreService(IRepository<GuildUser> user)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
        }

        [Command]
        public async Task Default()
        {
            var collection = _user.GetAll().Where(w => w.GuildId == Context.Guild.Id).OrderByDescending(f => f.Points);
            var pos = await collection.IndexOf(f => f.UserId == Context.User.Id && f.GuildId == Context.Guild.Id);
            var start = pos / 10 * 10;
            var list = await collection.Skip(start).Limit(10).ToList();
            var users = GetUsers(list);
            await ReplyAsync(GetScoreboardMessage(start, pos, list, users));
        }

        [Command]
        public async Task Scoreboard(int page)
        {
            var collection = _user.GetAll().Where(w => w.GuildId == Context.Guild.Id).OrderByDescending(f => f.Points);
            var pos = await collection.IndexOf(f => f.UserId == Context.User.Id && f.GuildId == Context.Guild.Id);
            var max = (int)await collection.CountDocumentsAsync() / 10;
            if (page > max)
                page = max;
            var start = page / 10 * 10;
            var list = await collection.Skip(start).Limit(10).ToList();
            var users = GetUsers(list);
            await ReplyAsync(GetScoreboardMessage(start, pos, list, users));
        }

        [Command("wipe")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ScoreboardWipe()
        {
            await _user.DeleteMany(f => f.GuildId == Context.Guild.Id);
            await ReplyAsync("```css\nGuild scoreboard has been wiped```");
        }

        [Command("left")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ScoreboardLeftUsers()
        {
            var i = 0;
            await _user.GetAll().Where(f => f.GuildId == Context.Guild.Id).ForEach(async f =>
            {
                if (Context.Guild.GetUser(f.UserId) == null)
                {
                    await _user.Delete(f.UserId);
                    i++;
                }
            });
            if (i > 0)
                await ReplyAsync($"```css\n{(i == 1 ? "a user" : $"[{i}] users were")} removed from the scoreboard```");
            else await ReplyAsync("There aren't any users who have left the guild.");
        }

        [Command("remove")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ScoreboardRemoveUser(IUser user)
        {
            var guildUser = _user.Delete(f => f.UserId == user.Id && f.GuildId == Context.Guild.Id);
            await ReplyAsync($"```css\n[{user.Username}] was removed from the scoreboard```");
        }

        [Command("give")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ScoreboardGivePoints(IUser user, ulong points)
        {
            var guildUser = await _user.GetByExpression(f => f.UserId == user.Id && f.GuildId == Context.Guild.Id);
            guildUser.Points += points;
            await _user.Update(guildUser, guildUser.Id);
            await ReplyAsync($"```css\n[{user.Username}] was given {(points == 1 ? "a point" : $"[{points}] pooints")}```");
        }

        [Command("take")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ScoreboardTakePoints(IUser user, ulong points)
        {
            var guildUser = await _user.GetByExpression(f => f.UserId == user.Id && f.GuildId == Context.Guild.Id);
            guildUser.Points -= points;
            await _user.Update(guildUser, guildUser.Id);
            await ReplyAsync($"```css\n[{user.Username}] had {(points == 1 ? "a point" : $"[{points}] pooints")} taken```");
        }

        private Tuple<string, ulong>[] GetUsers(List<GuildUser> list)
        {
            var users = new Tuple<string, ulong>[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                var user = Context.Guild.GetUser(list[i].UserId);
                var userName = user == null ? "User Left Guild (" + list[i].UserId + ")" : user.Username;
                users[i] = new Tuple<string, ulong>(userName, list[i].Points);
            }
            return users;
        }

        private string GetScoreboardMessage(int start, int pos, List<GuildUser> list, Tuple<string, ulong>[] users)
        {
            var builder = new StringBuilder($"**Guild scoreboard for __{Context.Guild.Name}__**\n```cs\n\"Top{start + 10}\"");
            for (var i = 0; i < list.Count; i++)
                builder.Append($"\n[{i + 1}] #{users[i].Item1}\nScore: {users[i].Item2}");
            builder.Append("\n-------------------------------------" +
                "\n# Your Guild Placing Stats" +
                $"\nRank: {pos + 1}    Total Score: {list[pos].Points}```");
            return builder.ToString();
        }
    }
}