using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    public class UserService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<AppUser> _user;

        public UserService(IRepository<AppUser> user)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
        }

        [Command("globalStats")]
        public async Task GlobalStats(IUser user = null)
        {
            var target = user ?? Context.User;
            if (target.IsBot)
            {
                await ReplyAsync("Bots don't have stats");
                return;
            }
            var account = await _user.GetById(target.Id);
            await ReplyAsync($"{target.Username} has {account.Points} xp and is level {account.LVL}");
        }
    }
}