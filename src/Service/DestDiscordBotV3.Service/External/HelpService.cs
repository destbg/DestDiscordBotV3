using DestDiscordBotV3.Model;
using Discord.Commands;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    [Group("help")]
    public class HelpService : ModuleBase<CommandContextWithPrefix>
    {
        [Command]
        public async Task MainHelp()
        {
            await ReplyAsync("```py\n'Commands List'```" +
                $"\nUse `{Context.Prefix}help <command>` to see more info on a command, for example `{Context.Prefix}help 8ball`" +
                "\n\n**Core: `ping` `math` `report` `wiki` `google` `randomperson` `scoreboard` `tag` `todo`**" +
                "\n**Fun: `8ball` `catfacts` `dogfacts` `fortune` `choose` `coin` `dice` `reverse` `rps` `time` `remindme`**" +
                "\n\n```bash\n# Don't include the example brackets when using commands!\n# To view mod commands, use " +
                $"{Context.Prefix}admin```");
        }

        [Command("help")]
        public async Task HelpHelp() =>
            await ReplyAsync("You need help with the help command?");
    }
}