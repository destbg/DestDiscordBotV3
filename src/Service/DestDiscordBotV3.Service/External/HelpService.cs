namespace DestDiscordBotV3.Service.External
{
    using Data;
    using Discord.Commands;
    using Model;
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Group("help")]
    public class HelpService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<AppHelp> _help;

        public HelpService(IRepository<AppHelp> help)
        {
            _help = help ?? throw new ArgumentNullException(nameof(help));
        }

        [Command]
        public async Task Main()
        {
            var builder = new StringBuilder("```py\n'Commands List'```" +
                $"\nUse `{Context.Prefix}help <command>` to see more info on a command, for example `{Context.Prefix}help 8ball`\n");

            foreach (var group in (await _help.GetAllToList()).GroupBy(g => g.HelpType))
            {
                builder.Append($"\n**{group.Key.ToString()}: ");
                foreach (var help in group)
                    builder.Append($" `{help.Id}`");
                builder.Append("**");
            }

            builder.Append("\n\n```bash\n# Don't include the example brackets when using commands!" +
                $"\n# To view mod commands, use {Context.Prefix}admin```");

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