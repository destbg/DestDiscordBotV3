using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.Interface;
using System;

namespace DestDiscordBotV3.Service.Internal
{
    public class ReportFactory : IReportFactory
    {
        public Report Create(string guild, string user, string message) =>
            new Report
            {
                Id = Guid.NewGuid(),
                Guild = guild,
                User = user,
                Message = message
            };
    }
}