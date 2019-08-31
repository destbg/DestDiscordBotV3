using DestDiscordBotV3.Model;
using System;

namespace DestDiscordBotV3.Common.NewUser
{
    public class NewUserFactory : INewUserFactory
    {
        public AppNewUser Create(ulong guildId, ulong channelId, ulong userId, ulong messageId) =>
            new AppNewUser
            {
                Id = channelId,
                GuildId = guildId,
                UserId = userId,
                MessageId = messageId,
                LastMessage = DateTime.UtcNow
            };
    }
}
