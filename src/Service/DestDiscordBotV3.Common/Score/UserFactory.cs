using DestDiscordBotV3.Model;
using System;

namespace DestDiscordBotV3.Common.Score
{
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