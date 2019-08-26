using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.Interface;
using System;

namespace DestDiscordBotV3.Service.Internal
{
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