namespace DestDiscordBotV3.Service.Internal
{
    using Model;
    using Service.Interface;
    using System;

    public class TodoFactory : ITodoFactory
    {
        public Todo Create(ulong userId, string msg) =>
            new Todo
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Msg = msg
            };
    }
}