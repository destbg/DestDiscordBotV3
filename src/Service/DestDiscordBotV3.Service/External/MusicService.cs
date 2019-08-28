using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.Extension;
using DestDiscordBotV3.Service.Interface;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    public class MusicService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IMusicHandler _musicHandler;
        private readonly IRepository<Music> _music;
        private readonly IMusicFactory _musicFactory;

        public MusicService(IMusicHandler musicHandler, IRepository<Music> music, IMusicFactory musicFactory)
        {
            _musicHandler = musicHandler ?? throw new ArgumentNullException(nameof(musicHandler));
            _music = music ?? throw new ArgumentNullException(nameof(music));
            _musicFactory = musicFactory ?? throw new ArgumentNullException(nameof(musicFactory));
        }

        [Command("Leave")]
        public async Task Leave()
        {
            var user = Context.User as SocketGuildUser;
            if (user.VoiceChannel is null)
            {
                await ReplyAsync("Please join the channel the bot is in to make it leave.");
            }
            else
            {
                await _musicHandler.LeaveAsync(user.VoiceChannel);
                await ReplyAsync($"Bot has now left {user.VoiceChannel.Name}");
            }
        }

        [Command("Play"), Priority(0)]
        public async Task Play([Remainder]string query)
        {
            var user = Context.User as SocketGuildUser;
            if (user.VoiceChannel is null)
            {
                await ReplyAsync("You need to connect to a voice channel.");
                return;
            }
            var (listTracks, message) = await _musicHandler.PlayAsync(user.VoiceChannel, Context.Channel as ITextChannel, query, Context.Guild.Id, Context.Prefix);
            if (listTracks)
            {
                await _music.Delete(Context.Guild.Id);
                var music = _musicFactory.Create(Context.Guild.Id, query);
                await _music.Create(music);
#pragma warning disable CS4014
                TimedFunction.RemoveMusic(_music, music.Id, 30);
#pragma warning restore CS4014
            }
            await ReplyAsync(message);
        }

        [Command("Play"), Priority(1)]
        public async Task Play(int track)
        {
            var user = Context.User as SocketGuildUser;
            if (user.VoiceChannel is null)
            {
                await ReplyAsync("You need to connect to a voice channel.");
                return;
            }
            if (track < 1 || track > 5)
            {
                await ReplyAsync("You can only choose between 1 to 5");
                return;
            }
            var music = await _music.GetByExpression(f => f.GuildId == Context.Guild.Id);
            await _music.Delete(Context.Guild.Id);
            var (_, message) = await _musicHandler.PlayAsync(user.VoiceChannel, Context.Channel as ITextChannel, music.Query, Context.Guild.Id, Context.Prefix, track - 1);
            await ReplyAsync(message);
        }

        [Command("Stop")]
        public async Task Stop()
            => await ReplyAsync(await _musicHandler.StopAsync(Context.Guild.Id));

        [Command("Skip")]
        public async Task Skip()
            => await ReplyAsync(await _musicHandler.SkipAsync(Context.Guild.Id));

        [Command("Shuffle")]
        public async Task Shuffle() =>
            await ReplyAsync(await _musicHandler.Shuffle(Context.Guild.Id));

        [Command("Pause")]
        public async Task Pause()
            => await ReplyAsync(await _musicHandler.PauseOrResumeAsync(Context.Guild.Id));

        [Command("Resume")]
        public async Task Resume()
            => await ReplyAsync(await _musicHandler.ResumeAsync(Context.Guild.Id));
    }
}