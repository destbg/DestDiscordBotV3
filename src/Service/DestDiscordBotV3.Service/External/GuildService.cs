using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.Extension;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    public class GuildService : ModuleBase<CommandContextWithPrefix>
    {
        [Command("purge"), Alias("delete", "prune")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task PurgeAsync(int num)
        {
            if (num > 99) num = 99;
            else if (num < 0) return;
            var messagesDelete = await Context.Channel.GetMessagesAsync(num + 1).FlattenAsync();
            await (Context.Channel as ITextChannel).DeleteMessagesAsync(messagesDelete);
#pragma warning disable CS4014
            TimedFunction.SendMessage(await ReplyAsync($"Purged {(num == 1 ? "a message" : $"{num} messages")} in {Context.Channel.Name}"), 5);
            #pragma warning restore CS4014
        }

        [Command("kick")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task Kick(IGuildUser user, string reason = "No reason provided.")
        {
            await user.KickAsync(reason);
            await ReplyAsync("```css\n[" + user.Username + "] got kicked!```");
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task Ban(IGuildUser user, int time = 5, string reason = "No reason provided.")
        {
            await user.Guild.AddBanAsync(user, time, reason);
            await ReplyAsync("```css\n[" + user.Username + "] got banned!```");
        }

        [Command("react")]
        [RequireUserPermission(GuildPermission.AddReactions)]
        [RequireBotPermission(GuildPermission.AddReactions)]
        public async Task React(ulong msgId, string emoji, IMessageChannel channel = null)
        {
            channel = channel ?? Context.Channel;
            await (await channel.GetMessageAsync(msgId) as IUserMessage).AddReactionAsync(new Emoji(emoji));
        }
    }
}
