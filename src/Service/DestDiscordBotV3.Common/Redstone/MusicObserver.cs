namespace DestDiscordBotV3.Common.Redstone
{
    using Data;
    using Data.Extension;
    using Model;
    using System;
    using System.Threading.Tasks;

    public class MusicObserver : IMusicObserver
    {
        private readonly IRepository<Music> _music;

        public MusicObserver(IRepository<Music> music)
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