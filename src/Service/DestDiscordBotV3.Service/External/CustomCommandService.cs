namespace DestDiscordBotV3.Service.External
{
    using Data;
    using Data.Extension;
    using Discord;
    using Discord.Commands;
    using Model;
    using Service.Interface;
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Group("customcommand"), Alias("cc", "cuscom")]
    [RequireUserPermission(GuildPermission.ManageMessages)]
    public class CustomCommandService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<AppGuild> _guild;
        private readonly ICustomCommandFactory _customCommandFactory;

        public CustomCommandService(IRepository<AppGuild> guild, ICustomCommandFactory customCommandFactory)
        {
            _guild = guild ?? throw new ArgumentNullException(nameof(guild));
            _customCommandFactory = customCommandFactory ?? throw new ArgumentNullException(nameof(customCommandFactory));
        }

        [Command]
        public async Task List()
        {
            var guild = await _guild.GetById(Context.Guild.Id);
            if (!guild.CustomCommands.Any())
            {
                await ReplyAsync("You don't have any custom commands on this server");
                return;
            }
            var builder = new StringBuilder($"List of custom commands for **{Context.Guild.Name}**!\n");
            foreach (var command in guild.CustomCommands)
                builder.Append($"[**{command.Command}**] {command.Message}");
            await ReplyAsync(builder.ToString());
        }

        [Command("create")]
        public async Task Create(string command, [Remainder] string message)
        {
            if (message.Length > 200)
            {
                await ReplyAsync("The length of the message can be up to 200 characters");
                return;
            }
            var count = await _guild.GetAll().Where(w => w.CustomCommands != null && w.Id == Context.Guild.Id).CountDocumentsAsync();
            if (count >= 20)
            {
                await ReplyAsync("You can only have up to 20 custom commands on a guild");
                return;
            }
            var guild = await _guild.GetById(Context.Guild.Id);
            if (guild.CustomCommands.FirstOrDefault(f => f.Command == command) != null)
            {
                await ReplyAsync($"The command {command} already exists");
                return;
            }
            guild.CustomCommands.Add(_customCommandFactory.Create(command, message));
            await _guild.Update(guild, guild.Id);
            await ReplyAsync($"Command {command} has been added to list of commands");
        }

        [Command("edit")]
        public async Task Edit(string command, [Remainder] string message)
        {
            if (message.Length > 200)
            {
                await ReplyAsync("The length of the message can be up to 200 characters");
                return;
            }
            var guild = await _guild.GetById(Context.Guild.Id);
            var customCommand = guild.CustomCommands.FirstOrDefault(f => f.Command == command);
            if (customCommand is null)
            {
                await ReplyAsync($"The command {command} doesn't exists");
                return;
            }
            guild.CustomCommands.Remove(customCommand);
            customCommand.Message = message;
            guild.CustomCommands.Add(customCommand);
            await _guild.Update(guild, guild.Id);
            await ReplyAsync($"Command {command} has been changed");
        }

        [Command("remove")]
        public async Task Remove(string command)
        {
            var guild = await _guild.GetById(Context.Guild.Id);
            var customCommand = guild.CustomCommands.FirstOrDefault(f => f.Command == command);
            if (customCommand is null)
            {
                await ReplyAsync($"The command {command} doesn't exists");
                return;
            }
            guild.CustomCommands.Remove(customCommand);
            await _guild.Update(guild, guild.Id);
            await ReplyAsync($"Command {command} has been removed from the list");
        }
    }
}