namespace DestDiscordBotV3.Service.External
{
    using Common.Function;
    using Discord;
    using Discord.Commands;
    using Model;
    using System.Threading.Tasks;

    public class GuildService : ModuleBase<CommandContextWithPrefix>
    {
        [Command("prune"), Alias("delete", "purge")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task Purge(int num)
        {
            if (num > 99) num = 99;
            else if (num < 0) return;
            var messagesDelete = await Context.Channel.GetMessagesAsync(num + 1).FlattenAsync();
            await (Context.Channel as ITextChannel).DeleteMessagesAsync(messagesDelete, new RequestOptions
            {
                AuditLogReason = $"{Context.User.Username} used prune to delete {num + 1} messages"
            });
            TimedFunction.SendMessage(await ReplyAsync($"Purged {(num == 1 ? "a message" : $"{num} messages")} in {Context.Channel.Name}"), 5);
        }

        [Command("kick")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task Kick(IGuildUser user, [Remainder] string reason = "No reason provided.")
        {
            await user.KickAsync(reason);
            await ReplyAsync("```css\n[" + user.Username + "] got kicked!```");
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task Ban(IGuildUser user, [Remainder] string reason = "No reason provided.")
        {
            await user.Guild.AddBanAsync(user, reason: reason);
            await ReplyAsync("```css\n[" + user.Username + "] got banned!```");
        }

        [Command("react")]
        [RequireUserPermission(GuildPermission.AddReactions)]
        [RequireBotPermission(GuildPermission.AddReactions)]
        public async Task React(ulong msgId, string emoji, IMessageChannel channel = null)
        {
            channel = channel ?? Context.Channel;
            await (await channel.GetMessageAsync(msgId) as IUserMessage).AddReactionAsync(new Emoji(emoji));
            await ReplyAsync("Reaction added successfully");
        }
    }
}