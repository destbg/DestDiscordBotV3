using DestDiscordBotV3.Service.Interface;
using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victoria;
using Victoria.Entities;

namespace DestDiscordBotV3.Service.Extension
{
    public class MusicHandler : IMusicHandler
    {
        private readonly LavaRestClient _lavaRestClient;
        private readonly LavaSocketClient _lavaSocketClient;
        private readonly DiscordSocketClient _client;

        public MusicHandler(LavaRestClient lavaRestClient, LavaSocketClient lavaSocketClient, DiscordSocketClient client)
        {
            _lavaRestClient = lavaRestClient ?? throw new ArgumentNullException(nameof(lavaRestClient));
            _lavaSocketClient = lavaSocketClient ?? throw new ArgumentNullException(nameof(lavaSocketClient));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public Task Initialize()
        {
            _client.Ready += ClientReadyAsync;
            _lavaSocketClient.Log += LogAsync;
            _lavaSocketClient.OnTrackFinished += TrackFinished;
            return Task.CompletedTask;
        }

        public async Task ConnectAsync(SocketVoiceChannel voiceChannel, ITextChannel textChannel)
            => await _lavaSocketClient.ConnectAsync(voiceChannel, textChannel);

        public async Task LeaveAsync(SocketVoiceChannel voiceChannel)
            => await _lavaSocketClient.DisconnectAsync(voiceChannel);

        public async Task<(bool, string)> PlayAsync(string query, ulong guildId, string prefix, int pos = -1)
        {
            var _player = _lavaSocketClient.GetPlayer(guildId);
            var results = await _lavaRestClient.SearchYouTubeAsync(query);
            if (results.LoadType == LoadType.NoMatches || results.LoadType == LoadType.LoadFailed)
                return (false, "No matches found.");

            LavaTrack track = null;

            if (Uri.TryCreate(query, UriKind.Absolute, out var uriResult) &&
                (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                track = results.Tracks.FirstOrDefault();

            var tracks = results.Tracks.ToArray();

            if (pos == -1 && track == null)
            {
                var builder = new StringBuilder($"**Do `{prefix}play 1-5` to choose one of the tracks!**\n");
                for (int i = 0; i < tracks.Length && i < 5; i++)
                    builder.Append($"**{i + 1}:** {tracks[i].Title} **({(tracks[i].Length.Hours > 0 ? tracks[i].Length.ToString(@"hh\:mm\:ss") : tracks[i].Length.ToString(@"mm\:ss"))})**\n");
                return (true, builder.ToString());
            }

            if (track == null)
                track = tracks[pos];

            if (_player.IsPlaying)
            {
                _player.Queue.Enqueue(track);
                return (false, $"{track.Title} has been added to the queue.");
            }
            else
            {
                await _player.PlayAsync(track);
                return (false, $"Now Playing: **{track.Title}**");
            }
        }

        public async Task<string> StopAsync(ulong guildId)
        {
            var _player = _lavaSocketClient.GetPlayer(guildId);
            if (_player is null)
                return "Error with Player";
            await _player.StopAsync();
            return "Music Playback Stopped.";
        }

        public async Task<string> SkipAsync(ulong guildId)
        {
            var _player = _lavaSocketClient.GetPlayer(guildId);
            if (_player is null || _player.Queue.Items.Any())
                return "Nothing in queue.";

            var oldTrack = _player.CurrentTrack;
            await _player.SkipAsync();
            return $"Skiped: {oldTrack.Title} \nNow Playing: {_player.CurrentTrack.Title}";
        }

        public async Task<string> SetVolumeAsync(int vol, ulong guildId)
        {
            var _player = _lavaSocketClient.GetPlayer(guildId);
            if (_player is null)
                return "Player isn't playing.";

            if (vol > 150 || vol <= 2)
            {
                return "Please use a number between 2 - 150";
            }

            await _player.SetVolumeAsync(vol);
            return $"Volume set to: {vol}";
        }

        public async Task<string> PauseOrResumeAsync(ulong guildId)
        {
            var _player = _lavaSocketClient.GetPlayer(guildId);
            if (_player is null)
                return "Player isn't playing.";

            if (!_player.IsPaused)
            {
                await _player.PauseAsync();
                return "Player is Paused.";
            }
            else
            {
                await _player.ResumeAsync();
                return "Playback resumed.";
            }
        }

        public async Task<string> ResumeAsync(ulong guildId)
        {
            var _player = _lavaSocketClient.GetPlayer(guildId);
            if (_player is null)
                return "Player isn't playing.";

            if (_player.IsPaused)
            {
                await _player.ResumeAsync();
                return "Playback resumed.";
            }

            return "Player is not paused.";
        }

        private async Task ClientReadyAsync()
        {
            await _lavaSocketClient.StartAsync(_client);
        }

        private async Task TrackFinished(LavaPlayer player, LavaTrack track, TrackEndReason reason)
        {
            if (!reason.ShouldPlayNext())
                return;

            if (!player.Queue.TryDequeue(out var item) || !(item is LavaTrack nextTrack))
            {
                await player.TextChannel.SendMessageAsync("There are no more tracks in the queue.");
                return;
            }

            await player.PlayAsync(nextTrack);
        }

        private Task LogAsync(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.Message);
            return Task.CompletedTask;
        }
    }
}
