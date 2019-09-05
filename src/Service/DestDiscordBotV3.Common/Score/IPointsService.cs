namespace DestDiscordBotV3.Common.Score
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IPointsService" />
    /// </summary>
    public interface IPointsService
    {
        /// <summary>
        /// Give points to a user
        /// </summary>
        Task GivePoints(ulong userId, ulong guildId);
    }
}