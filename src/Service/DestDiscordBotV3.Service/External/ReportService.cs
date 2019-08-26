using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.Interface;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    [Group("report")]
    public class ReportService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<Report> _report;
        private readonly IReportFactory _reportFactory;

        public ReportService(IRepository<Report> report, IReportFactory reportFactory)
        {
            _report = report ?? throw new ArgumentNullException(nameof(report));
            _reportFactory = reportFactory ?? throw new ArgumentNullException(nameof(reportFactory));
        }

        [Command]
        public async Task ReportAsync([Remainder] string message)
        {
            await _report.Create(_reportFactory.Create(Context.Guild.Name, Context.User.Username, message));
            await ReplyAsync("You successfully made a report!");
        }
    }
}