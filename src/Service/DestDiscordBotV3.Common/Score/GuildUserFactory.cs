using DestDiscordBotV3.Model;
using System;

namespace DestDiscordBotV3.Common.Score
{
    public class GuildUserFactory : IGuildUserFactory
    {
        public GuildUser Create(ulong userId, ulong guildId, ulong points) =>
            new GuildUser
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                GuildId = guildId,
                Points = points
            };
    }
}