using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Common.Score
{
    public class PointsService : IPointsService
    {
        private readonly IRepository<AppUser> _user;
        private readonly IRepository<GuildUser> _guildUser;
        private readonly IUserFactory _userFactory;
        private readonly IGuildUserFactory _guildUserFactory;

        public PointsService(IRepository<AppUser> user, IRepository<GuildUser> guildUser, IUserFactory userFactory, IGuildUserFactory guildUserFactory)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
            _guildUser = guildUser ?? throw new ArgumentNullException(nameof(guildUser));
            _userFactory = userFactory ?? throw new ArgumentNullException(nameof(userFactory));
            _guildUserFactory = guildUserFactory ?? throw new ArgumentNullException(nameof(guildUserFactory));
        }

        public async Task GivePoints(ulong userId, ulong guildId)
        {
            var points = await CheckUser(userId);
            await CheckGuildUser(userId, guildId, points);
        }

        private async Task<ulong> CheckUser(ulong userId)
        {
            AppUser user;
            try
            {
                user = await _user.GetById(userId);
            }
            catch
            {
                user = null;
            }
            if (user == null)
            {
                await _user.Create(_userFactory.Create(userId));
                return 10;
            }
            var points = CalculatePoints(user);
            user.Points += points;
            user.LastMessage = DateTime.UtcNow;
            await _user.Update(user, user.Id);
            return points;
        }

        private async Task CheckGuildUser(ulong userId, ulong guildId, ulong points)
        {
            GuildUser guildUser;
            try
            {
                guildUser = await _guildUser.GetByExpression(f => f.UserId == userId && f.GuildId == guildId);
            }
            catch
            {
                guildUser = null;
            }
            if (guildUser == null)
            {
                await _guildUser.Create(_guildUserFactory.Create(userId, guildId, points));
            }
            else
            {
                guildUser.Points += points;
                await _guildUser.Update(guildUser, guildUser.Id);
            }
        }

        private ulong CalculatePoints(AppUser user)
        {
            var difference = DateTime.UtcNow.Subtract(user.LastMessage);
            return difference.TotalSeconds > 250 ? 50 : (ulong)(difference.TotalSeconds / 5);
        }
    }
}