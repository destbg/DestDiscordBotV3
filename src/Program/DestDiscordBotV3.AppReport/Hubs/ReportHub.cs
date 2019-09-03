using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3.AppReport.Hubs
{
    public class ReportHub : Hub
    {
        private readonly IRepository<Report> _report;

        public ReportHub(IRepository<Report> report)
        {
            _report = report ?? throw new ArgumentNullException(nameof(report));
        }

        public async Task GiveReports()
        {
            var reports = await _report.GetAllToList();
            await Clients.Caller.SendAsync("ReportsRecieved", reports);
        }

        public async Task RemoveReports(Guid id) =>
            await _report.Delete(id);
    }
}
