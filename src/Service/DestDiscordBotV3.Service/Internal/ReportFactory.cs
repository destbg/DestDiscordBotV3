namespace DestDiscordBotV3.Service.Internal
{
    using Model;
    using Service.Interface;
    using System;

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