using DestDiscordBotV3.Model;
using Discord.Commands;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    [Group("admin")]
    public class AdminHelpService : ModuleBase<CommandContextWithPrefix>
    {
        [Command]
        public async Task AllHelp() =>
            await ReplyAsync("```fix\n'Admin Commands List'```\n" +
                $"Use `{Context.Prefix}admin <command>` to see more info on a command, for example `{Context.Prefix}admin 8ball`" +
                "\n\n**Configuration: `selfroles` `8ball` `prefix` `welcome` `autorole` `channel`**" +
                "\n**Server Management: `purge` `scoreboard` `customcommands` `giveaway` `kick` `ban`**" +
                "\n\n```bash\n# Don't include the example brackets when using commands!\n# To view standard commands, use " +
                $"{Context.Prefix}help```");
    }
}