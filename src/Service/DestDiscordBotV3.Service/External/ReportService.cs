namespace DestDiscordBotV3.Service.External
{
    using Data;
    using Discord.Commands;
    using Model;
    using Service.Interface;
    using System;
    using System.Threading.Tasks;

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
        public async Task Report([Remainder] string message)
        {
            await _report.Create(_reportFactory.Create(Context.Guild.Name, Context.User.Username, message));
            await ReplyAsync("You successfully made a report!");
        }
    }
}