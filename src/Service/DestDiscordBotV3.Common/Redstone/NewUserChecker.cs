using DestDiscordBotV3.Common.Extension;
using DestDiscordBotV3.Data;
using DestDiscordBotV3.Data.Extension;
using DestDiscordBotV3.Model;
using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Common.Redstone
{
    public class NewUserChecker : INewUserChecker
    {
        private readonly IRepository<AppNewUser> _newUser;
        private readonly IRepository<AppGuild> _guild;
        private readonly DiscordSocketClient _client;

        public NewUserChecker(IRepository<AppNewUser> newUser, IRepository<AppGuild> guild, DiscordSocketClient client)
        {
            _newUser = newUser ?? throw new ArgumentNullException(nameof(newUser));
            _guild = guild ?? throw new ArgumentNullException(nameof(guild));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task CheckNewUsersAsync() => 
            await _newUser.GetAll().ForEach(async user =>
            {
                if (!(_client.GetChannel(user.Id) is ITextChannel channel))
                {
                    await _newUser.Delete(user.Id);
                    return;
                }
                if (DateTime.UtcNow.Subtract(user.LastMessage).TotalMinutes > 50)
                {
                    var guild = _client.GetGuild(channel.GuildId);
                    var guildUser = guild.GetUser(user.UserId);
                    if (guildUser is null)
                    {
                        await _newUser.Delete(user.Id);
                        return;
                    }
                    var appGuild = await _guild.GetById(guild.Id);
                    if (appGuild.JoinSystem is null)
                    {
                        var roleGive = guild.Roles.OrderByDescending(f => f.Members).FirstOrDefault();
                        if (roleGive is null)
                            return;
                        await guildUser.AddRoleAsync(roleGive);
                        return;
                    }
                    if (!appGuild.JoinSystem.DefaultRole.HasValue)
                        return;
                    var role = guild.GetRole(appGuild.JoinSystem.DefaultRole.Value);
                    await guildUser.AddRoleAsync(role);

                    await channel.DeleteAsync();
                    await DefaultChannel.Get(guild).SendMessageAsync($"User {guildUser.Username} was inactive for 1 hour and was given the default role");
                }
            });
    }
}
