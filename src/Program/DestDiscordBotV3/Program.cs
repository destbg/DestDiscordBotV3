namespace DestDiscordBotV3
{
    using System.Threading.Tasks;

    internal class Program
    {
        private static async Task Main()
        {
            var dInjection = new DInjection();
            await StartUp.DoChecks(dInjection);
            var connection = dInjection.Resolve<IConnection>();
            await connection.ConnectAsync(StartUp.GetToken());
        }
    }
}