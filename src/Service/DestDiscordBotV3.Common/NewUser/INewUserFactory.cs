﻿using DestDiscordBotV3.Model;

namespace DestDiscordBotV3.Common.NewUser
{
    public interface INewUserFactory
    {
        AppNewUser Create(ulong channelId, ulong userId, ulong messageId);
    }
}