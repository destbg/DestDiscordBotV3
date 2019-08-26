using System.Threading.Tasks;

namespace DestDiscordBotV3
{
    internal class Program
    {
        private static void Main() =>
            new Program().StartAsync().GetAwaiter().GetResult();

        private async Task StartAsync()
        {
            var dInjection = new DInjection();
            await StartUp.DoChecks(dInjection);
            var connection = dInjection.Resolve<IConnection>();
            await connection.ConnectAsync(StartUp.GetToken());
        }
    }
}