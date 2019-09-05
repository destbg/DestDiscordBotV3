namespace DestDiscordBotV3.Service.Interface
{
    using Model;

    /// <summary>
    /// Defines the <see cref="IReportFactory" />
    /// </summary>
    public interface IReportFactory
    {
        /// <summary>
        /// Create an <see cref="Report"/> class from the specified arguments
        /// </summary>
        Report Create(string guild, string user, string message);
    }
}