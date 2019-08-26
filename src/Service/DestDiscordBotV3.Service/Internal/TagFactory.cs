using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.Interface;
using System;

namespace DestDiscordBotV3.Service.Internal
{
    public class TagFactory : ITagFactory
    {
        public Tag Create(ulong userId, string tag, string message) =>
            new Tag
            {
                Id = Guid.NewGuid(),
                Msg = message,
                Name = tag,
                UserId = userId
            };
    }
}