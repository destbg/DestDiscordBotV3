using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.Interface;
using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    [Group("joinsystem")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class NewUserService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<AppGuild> _guild;
        private readonly IJoinSystemFactory _joinSystem;

        public NewUserService(IRepository<AppGuild> guild, IJoinSystemFactory joinSystem)
        {
            _guild = guild ?? throw new ArgumentNullException(nameof(guild));
            _joinSystem = joinSystem ?? throw new ArgumentNullException(nameof(joinSystem));
        }

        [Command("setup")]
        public async Task NewUserSystemAsync(int rolesRequied, string doneMessage, IRole joinRole, IRole finishRole, params IRole[] roles)
        {
            if (doneMessage.Split(' ').Length > 1 && doneMessage.Length < 11)
            {
                await ReplyAsync("Done message needs to be a single word and not longer then 10 characters");
                return;
            }
            var guild = await _guild.GetById(Context.Guild.Id);
            guild.JoinSystem = _joinSystem.Create(rolesRequied, doneMessage, joinRole.Id, finishRole.Id, roles.Select(s => s.Id).ToArray());
            await _guild.Update(guild, guild.Id);
            await ReplyAsync("Set up of join system is now complete");
        }

        [Command("add")]
        public async Task AddRoleAsync(IRole role)
        {
            var guild = await _guild.GetById(Context.Guild.Id);
            if (guild.JoinSystem is null)
            {
                await ReplyAsync($"Use `{Context.Prefix}joinsystem setup` before this");
                return;
            }
            guild.JoinSystem.Roles.Add(role.Id);
            await _guild.Update(guild, guild.Id);
            await ReplyAsync($"Added **{role.Name}** to list of roles");
        }

        [Command("remove")]
        public async Task RemoveRoleAsync(IRole role)
        {
            var guild = await _guild.GetById(Context.Guild.Id);
            if (guild.JoinSystem is null)
            {
                await ReplyAsync($"Use `{Context.Prefix}joinsystem setup` before this");
                return;
            }
            guild.JoinSystem.Roles.Remove(role.Id);
            await _guild.Update(guild, guild.Id);
            await ReplyAsync($"Removed **{role.Name}** from the list of roles");
        }

        [Command("default")]
        public async Task DefaultAsync(IRole role)
        {
            var guild = await _guild.GetById(Context.Guild.Id);
            if (guild.JoinSystem is null)
            {
                await ReplyAsync($"Use `{Context.Prefix}joinsystem setup` before this");
                return;
            }
            guild.JoinSystem.DefaultRole = role.Id;
            await _guild.Update(guild, guild.Id);
            await ReplyAsync($"Default role is now **{role.Name}**");
        }

        [Command("join")]
        public async Task JoinRoleAsync(IRole role)
        {
            var guild = await _guild.GetById(Context.Guild.Id);
            if (guild.JoinSystem is null)
            {
                await ReplyAsync($"Use `{Context.Prefix}joinsystem setup` before this");
                return;
            }
            guild.JoinSystem.JoinRole = role.Id;
            await _guild.Update(guild, guild.Id);
            await ReplyAsync($"Join role role is now **{role.Name}**");
        }

        [Command("finish")]
        public async Task FinishRoleAsync(IRole role)
        {
            var guild = await _guild.GetById(Context.Guild.Id);
            if (guild.JoinSystem is null)
            {
                await ReplyAsync($"Use `{Context.Prefix}joinsystem setup` before this");
                return;
            }
            guild.JoinSystem.FinishRole = role.Id;
            await _guild.Update(guild, guild.Id);
            await ReplyAsync($"Finish role role is now **{role.Name}**");
        }

        [Command("done")]
        public async Task DoneMessageAsync(string message)
        {
            if (message.Split(' ').Length > 1 && message.Length < 11)
            {
                await ReplyAsync("Done message needs to be a single word and not longer then 10 characters");
                return;
            }
            var guild = await _guild.GetById(Context.Guild.Id);
            if (guild.JoinSystem is null)
            {
                await ReplyAsync($"Use `{Context.Prefix}joinsystem setup` before this");
                return;
            }
            guild.JoinSystem.DoneMessage = message;
            await _guild.Update(guild, guild.Id);
            await ReplyAsync($"Done message is now **{message}**");
        }

        [Command("rolesrequired")]
        public async Task RolesRequiedAsync(int rolesRequied)
        {
            var guild = await _guild.GetById(Context.Guild.Id);
            if (guild.JoinSystem is null)
            {
                await ReplyAsync($"Use `{Context.Prefix}joinsystem setup` before this");
                return;
            }
            guild.JoinSystem.RolesRequired = rolesRequied;
            await _guild.Update(guild, guild.Id);
            await ReplyAsync($"Roles required is now **{rolesRequied}**");
        }

        [Command("stop")]
        public async Task StopJoinSystem()
        {
            var guild = await _guild.GetById(Context.Guild.Id);
            guild.JoinSystem = null;
            await _guild.Update(guild, guild.Id);
            await ReplyAsync("Join system is no longer being used");
        }
    }
}
