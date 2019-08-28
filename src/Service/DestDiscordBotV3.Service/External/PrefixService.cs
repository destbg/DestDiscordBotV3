using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    [Group("prefix"), Alias("changePrefix", "newPrefix", "pref")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PrefixService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<AppGuild> _guild;

        public PrefixService(IRepository<AppGuild> guild)
        {
            _guild = guild ?? throw new ArgumentNullException(nameof(guild));
        }

        [Command("default")]
        public async Task DefaultAsync()
        {
            await ChangePrefix("dest!");
            await ReplyAsync("```css\nPrefix was changed to [dest!]```");
        }

        [Command("change")]
        public async Task ChangeAsync(string prefix)
        {
            await ChangePrefix(prefix);
            await ReplyAsync($"```css\nPrefix was changed to [{prefix}]```");
        }

        private async Task ChangePrefix(string prefix)
        {
            var guild = await _guild.GetById(Context.Guild.Id);
            guild.Prefix = prefix;
            await _guild.Update(guild, guild.Id);
        }
    }
}