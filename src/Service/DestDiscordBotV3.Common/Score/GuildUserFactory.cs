namespace DestDiscordBotV3.Common.Score
{
    using Model;
    using System;

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