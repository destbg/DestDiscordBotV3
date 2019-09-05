namespace DestDiscordBotV3.Service.Internal
{
    using Model;
    using Service.Interface;
    using System;

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