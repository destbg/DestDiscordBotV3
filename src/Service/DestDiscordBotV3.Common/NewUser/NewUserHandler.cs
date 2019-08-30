using DestDiscordBotV3.Common.Extension;
using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Common.NewUser
{
    public class NewUserHandler : INewUserHandler
    {
        private readonly IRepository<AppNewUser> _newUser;
        private readonly IRepository<AppGuild> _guild;
        private readonly INewUserFactory _newUserFactory;

        public NewUserHandler(IRepository<AppNewUser> newUser, IRepository<AppGuild> guild, INewUserFactory newUserFactory)
        {
            _newUser = newUser ?? throw new ArgumentNullException(nameof(newUser));
            _guild = guild ?? throw new ArgumentNullException(nameof(guild));
            _newUserFactory = newUserFactory ?? throw new ArgumentNullException(nameof(newUserFactory));
        }

        public async Task UserJoined(SocketGuildUser user)
        {
            var appGuild = await _guild.GetById(user.Guild.Id);
            if (appGuild.JoinSystem is null)
                return;

            if (user.IsBot)
            {
                await SendMessageToDefaultChannel(user.Guild,
                    $"User {user.Mention} is a bot so a channel wasn't created!");
                return;
            }

            if (user.Guild.CurrentUser.GuildPermissions.ManageChannels == false)
            {
                await SendMessageToDefaultChannel(user.Guild,
                    $"I couldn't create a channel for **{user.Username}** because i most likely don't have `Manage Channels` permission!");
                return;
            }

            var joinRole = user.Guild.GetRole(appGuild.JoinSystem.JoinRole);

            if (joinRole is null)
            {
                await SendMessageToDefaultChannel(user.Guild,
                    $"Join role is invalid");
                return;
            }

            if (user.Guild.GetRole(appGuild.JoinSystem.FinishRole) is null ||
                appGuild.JoinSystem.DefaultRole.HasValue &&
                user.Guild.GetRole(appGuild.JoinSystem.DefaultRole.Value) is null)
            {
                await SendMessageToDefaultChannel(user.Guild,
                    "Finish role or default role is invalid");
                return;
            }

            if (!joinRole.IsEveryone)
                await user.AddRoleAsync(joinRole);

            var channel = await user.Guild.CreateTextChannelAsync(user.Username, options:
                new RequestOptions { AuditLogReason = $"User {user.Username} joined!" });
            await channel.AddPermissionOverwriteAsync(user.Guild.CurrentUser,
                new OverwritePermissions(viewChannel: PermValue.Allow, sendMessages: PermValue.Allow));
            await channel.AddPermissionOverwriteAsync(user,
                new OverwritePermissions(viewChannel: PermValue.Allow, sendMessages: PermValue.Allow));

            var roles = GetRoles(appGuild.JoinSystem.Roles, user.Guild).Select(s => s.Name);

            if (!roles.Any())
            {
                await SendMessageToDefaultChannel(user.Guild,
                    $"I couldn't create a channel for **{user.Username}** because there aren't any valid roles in the list!");
                return;
            }

            var message = await channel.SendMessageAsync(CreateMessage(roles, appGuild.JoinSystem.DoneMessage));

            await _newUser.Create(_newUserFactory.Create(channel.Id, user.Id, message.Id));
        }

        public async Task<bool> NewUserMessage(CommandContextWithPrefix msg)
        {
            AppNewUser newUser;
            try
            {
                newUser = await _newUser.GetById(msg.Channel.Id);
            }
            catch
            {
                newUser = null;
            }
            if (newUser is null)
                return false;

            var appGuild = await _guild.GetById(msg.Guild.Id);

            if (appGuild.JoinSystem is null)
            {
                await SendMessageToDefaultChannel(msg.Guild,
                    "Join system was changed while a new user was using it");
                await SaveNewUser(msg);
                return true;
            }

            if (msg.Message.Content == appGuild.JoinSystem.DoneMessage)
            {
                if ((msg.User as SocketGuildUser).Roles.Count - 1 < appGuild.JoinSystem.RolesRequired)
                {
                    var wariningMessage = await msg.Channel.SendMessageAsync($"You need to add at least {appGuild.JoinSystem.RolesRequired} roles before you can finish");
                    await Task.Delay(3000);
                    await wariningMessage.DeleteAsync();
                    return true;
                }
                var finishRole = msg.Guild.GetRole(appGuild.JoinSystem.FinishRole);
                if (finishRole is null)
                {
                    await SendMessageToDefaultChannel(msg.Guild,
                        "New user tried to finish selecting but the finish role was invalid");
                    await SaveNewUser(msg);
                    return true;
                }
                if (!finishRole.IsEveryone)
                    await (msg.User as SocketGuildUser).AddRoleAsync(finishRole);
                await (msg.Channel as ITextChannel).DeleteAsync();
                await _newUser.Delete(msg.Channel.Id);
                return true;
            }

            var roles = GetRoles(appGuild.JoinSystem.Roles, msg.Guild);

            if (!roles.Any())
            {
                await SendMessageToDefaultChannel(msg.Guild,
                    "All listed roles were deleted while a new user was choosing them");
                await SaveNewUser(msg);
                return true;
            }

            SocketRole roleName = null;
            var content = msg.Message.Content.ToLower();
            foreach (var role in roles)
                if (content == role.Name.ToLower())
                {
                    roleName = role;
                    break;
                }

            if (roleName is null)
            {
                await msg.Message.DeleteAsync();
                return true;
            }

            var message = await msg.Channel.GetMessageAsync(newUser.MessageId) as IUserMessage;
            await message.ModifyAsync(f => f.Content = message.Content.Replace("- " + roleName.Name, "+ " + roleName.Name));
            await (msg.User as SocketGuildUser).AddRoleAsync(roleName);
            await msg.Message.DeleteAsync();
            newUser.LastMessage = DateTime.UtcNow;
            await _newUser.Update(newUser, newUser.Id);
            return true;
        }

        private string CreateMessage(IEnumerable<string> roles, string doneMessage) =>
            $"```diff\n- {string.Join("\n- ", roles)}```\nUse **{doneMessage}** to finish the set up!";

        private async Task SendMessageToDefaultChannel(SocketGuild guild, string message)
        {
            var defaultChannel = DefaultChannel.Get(guild);
            await defaultChannel.SendMessageAsync(message);
        }

        private async Task SaveNewUser(CommandContextWithPrefix msg)
        {
            var role = msg.Guild.Roles.OrderByDescending(f => f.Members).FirstOrDefault();
            if (role is null)
                return;

            await (msg.User as SocketGuildUser).AddRoleAsync(role);
            await _newUser.Delete(msg.Channel.Id);
        }

        private IEnumerable<SocketRole> GetRoles(IEnumerable<ulong> roles, SocketGuild guild) =>
            roles.Select(s => guild.GetRole(s))
                .Where(w => w != null);
    }
}
