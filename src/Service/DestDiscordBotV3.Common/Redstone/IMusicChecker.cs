﻿using System.Threading.Tasks;

namespace DestDiscordBotV3.Common.Redstone
{
    public interface IMusicChecker
    {
        Task TenSecondsPassedAsync();
    }
}