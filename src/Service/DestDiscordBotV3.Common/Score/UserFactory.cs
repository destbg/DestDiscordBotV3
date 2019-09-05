namespace DestDiscordBotV3.Common.Score
{
    using Model;
    using System;

    public class UserFactory : IUserFactory
    {
        public AppUser Create(ulong userId) =>
            new AppUser
            {
                Id = userId,
                Points = 10,
                LastMessage = DateTime.UtcNow
            };
    }
}