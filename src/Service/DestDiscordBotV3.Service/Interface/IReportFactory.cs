using DestDiscordBotV3.Model;

namespace DestDiscordBotV3.Service.Interface
{
    public interface IReportFactory
    {
        Report Create(string guild, string user, string message);
    }
}