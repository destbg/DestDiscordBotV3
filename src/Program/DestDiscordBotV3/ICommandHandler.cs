﻿using Discord.WebSocket;
using System.Threading.Tasks;

namespace DestDiscordBotV3
{
    public interface ICommandHandler
    {
        Task Initialize(DiscordSocketClient client);
    }
}