using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using Discord.Commands;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    [Group("admin")]
    public class AdminHelpService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<AppAdminHelp> _help;

        public AdminHelpService(IRepository<AppAdminHelp> help)
        {
            _help = help ?? throw new ArgumentNullException(nameof(help));
        }

        [Command]
        public async Task Main()
        {
            var builder = new StringBuilder("```fix\n'Admin Commands List'```" +
                $"\nUse `{Context.Prefix}admin <command>` to see more info on a command, for example `{Context.Prefix}help scoreboard`" +
                "\n\n**Commands:** ");

            foreach (var help in await _help.GetAllToList())
                builder.Append($" `{help.Id}`");

            builder.Append("\n\n```bash\n# Don't include the example brackets when using commands!" +
                $"\n# To view standart commands, use {Context.Prefix}help```");

            await ReplyAsync(builder.ToString());
        }

        [Command]
        public async Task Display(string command)
        {
            var help = await _help.GetById(command.ToLower());
            if (help is null)
            {
                await ReplyAsync("That command doesn't exist");
                return;
            }

            var helpPrefix = Context.Prefix + help.Id;
            var builder = new StringBuilder($"**`{helpPrefix}`** __`{help.Description}`__");

            if (help.Aliases != null)
            {
                builder.Append("\n**Aliases**:");
                foreach (var alias in help.Aliases)
                    builder.Append($" `{alias}`");
            }

            builder.Append("\n\n**Usage**:");
            if (help.Usages.Count == 1)
                builder.Append($" {helpPrefix} {help.Usages[0]}");
            else foreach (var usage in help.Usages)
                    builder.Append($"\n`{helpPrefix} {usage}`");

            if (help.Examples != null)
            {
                builder.Append("\n\n**Examples:**");
                foreach (var example in help.Examples)
                    builder.Append($"\n**`{helpPrefix} {example}`**");
            }

            await ReplyAsync(builder.ToString());
        }
    }
}