using DestDiscordBotV3.Model;
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
        private readonly IMusicHandler _music;

        public MusicService(IMusicHandler music)
        {
            _music = music ?? throw new ArgumentNullException(nameof(music));
        }

        [Command("Join")]
        public async Task Join()
        {
            var user = Context.User as SocketGuildUser;
            if (user.VoiceChannel is null)
            {
                await ReplyAsync("You need to connect to a voice channel.");
                return;
            }
            await _music.Initialize();
            await _music.ConnectAsync(user.VoiceChannel, Context.Channel as ITextChannel);
            await ReplyAsync($"now connected to {user.VoiceChannel.Name}");
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
                await ReplyAsync($"Bot has now left {user.VoiceChannel.Name}");
            }
        }

        [Command("Play")]
        public async Task Play([Remainder]string query)
        {
            await ReplyAsync(await _music.PlayAsync(query, Context.Guild.Id));
        }


        [Command("Stop")]
        public async Task Stop()
            => await ReplyAsync(await _music.StopAsync(Context.Guild.Id));

        [Command("Skip")]
        public async Task Skip()
            => await ReplyAsync(await _music.SkipAsync(Context.Guild.Id));

        [Command("Volume")]
        public async Task Volume(int vol)
            => await ReplyAsync(await _music.SetVolumeAsync(vol, Context.Guild.Id));

        [Command("Pause")]
        public async Task Pause()
            => await ReplyAsync(await _music.PauseOrResumeAsync(Context.Guild.Id));

        [Command("Resume")]
        public async Task Resume()
            => await ReplyAsync(await _music.ResumeAsync(Context.Guild.Id));
    }
}