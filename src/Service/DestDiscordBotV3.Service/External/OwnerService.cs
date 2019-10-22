namespace DestDiscordBotV3.Service.External
{
    using DestDiscordBotV3.Model;
    using Discord;
    using Discord.Commands;
    using System.Linq;
    using System.Threading.Tasks;

    [RequireOwner]
    public class OwnerService : ModuleBase<CommandContextWithPrefix>
    {
        [Command("tell")]
        public async Task TellSomeone(ulong userId, [Remainder] string message)
        {
            var user = Context.Client.GetUser(userId);
            if (user is null)
            {
                await ReplyAsync("I can't find that user.");
                return;
            }
            await user.SendMessageAsync(message);
            await ReplyAsync("Message sent");
        }

        [Command("listdms")]
        public async Task ListDMs(ulong userId, int amount = 10)
        {
            var user = Context.Client.GetUser(userId);
            if (user is null)
            {
                await ReplyAsync("I can't find that user.");
                return;
            }
            var channel = await user.GetOrCreateDMChannelAsync();
            var list = (await channel.GetMessagesAsync(amount).FlattenAsync())
                    .Select(s => $"**{s.Author.Username}**: {s.Content}");
            if (list.Any())
                await ReplyAsync(string.Join("\n", list));
            else await ReplyAsync("This DM channel doesn't have any messages.");
        }
    }
}