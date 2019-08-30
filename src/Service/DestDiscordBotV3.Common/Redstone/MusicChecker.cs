using DestDiscordBotV3.Data;
using DestDiscordBotV3.Data.Extension;
using DestDiscordBotV3.Model;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Common.Redstone
{
    public class MusicChecker : IMusicChecker
    {
        private readonly IRepository<Music> _music;

        public MusicChecker(IRepository<Music> music)
        {
            _music = music ?? throw new ArgumentNullException(nameof(music));
        }

        public async Task TenSecondsPassedAsync() =>
            await _music.GetAll().ForEach(async music =>
            {
                if (DateTime.UtcNow.Subtract(music.Requested).TotalSeconds < 11)
                    return;
                await _music.Delete(music.Id);
            });
    }
}
