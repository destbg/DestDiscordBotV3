using DestDiscordBotV3.Common.Logging;
using DestDiscordBotV3.Model;
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
        private readonly IDiscordLogger _logger;

        public MusicHandler(LavaRestClient lavaRestClient, LavaSocketClient lavaSocketClient, DiscordSocketClient client, IDiscordLogger logger)
        {
            _lavaRestClient = lavaRestClient ?? throw new ArgumentNullException(nameof(lavaRestClient));
            _lavaSocketClient = lavaSocketClient ?? throw new ArgumentNullException(nameof(lavaSocketClient));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Initialize()
        {
            _client.Ready += ClientReadyAsync;
            _lavaSocketClient.OnTrackFinished += TrackFinished;
            _lavaSocketClient.Log += _logger.Log;
            return Task.CompletedTask;
        }

        public async Task UserVoiceStateUpdated(SocketUser user, SocketVoiceState lastVoice, SocketVoiceState newVoice)
        {
            if (lastVoice.VoiceChannel != null && lastVoice.VoiceChannel.GetUser(_client.CurrentUser.Id) != null)
            {
                var player = _lavaSocketClient.GetPlayer(lastVoice.VoiceChannel.Guild.Id);
                if (player != null && !lastVoice.VoiceChannel.Users.Where(w => !w.IsBot).Any() && !player.IsPaused)
                    await player.PauseAsync();
            }
            else if (newVoice.VoiceChannel != null && newVoice.VoiceChannel.GetUser(_client.CurrentUser.Id) != null)
            {
                var player = _lavaSocketClient.GetPlayer(newVoice.VoiceChannel.Guild.Id);
                if (player != null && newVoice.VoiceChannel.Users.Where(w => !w.IsBot).Any() && player.IsPaused)
                    await player.ResumeAsync();
            }
        }

        public async Task LeaveAsync(SocketVoiceChannel voiceChannel)
            => await _lavaSocketClient.DisconnectAsync(voiceChannel);

        public async Task<(bool, string)> PlayAsync(SocketVoiceChannel voiceChannel, ITextChannel textChannel, string query, ulong guildId, string prefix, int pos = -1)
        {
            var player = _lavaSocketClient.GetPlayer(guildId);
            if (player is null)
            {
                await _lavaSocketClient.ConnectAsync(voiceChannel, textChannel);
                player = _lavaSocketClient.GetPlayer(guildId);
            }
            LavaTrack track = null;
            SearchResult results = null;
            var uriResult = GetUriResult(query);
            switch (uriResult)
            {
                case UriResult.PlayList:
                    results = await _lavaRestClient.SearchTracksAsync(query, true);
                    return (false, await MakePlayList(player, results));

                case UriResult.YoutubeLink:
                    results = await _lavaRestClient.SearchYouTubeAsync(query);
                    track = results.Tracks.FirstOrDefault();
                    break;

                case UriResult.SoundCloud:
                    results = await _lavaRestClient.SearchSoundcloudAsync(query);
                    track = results.Tracks.FirstOrDefault();
                    break;

                default:
                    results = await _lavaRestClient.SearchYouTubeAsync(query);
                    break;
            }
            if (results.LoadType == LoadType.NoMatches || results.LoadType == LoadType.LoadFailed)
                return (false, "No matches found.");

            var tracks = results.Tracks.ToArray();

            if (pos == -1 && track == null)
                return (true, MakeTrackList(prefix, tracks));

            if (track == null)
                track = tracks[pos];

            if (player.IsPlaying)
            {
                player.Queue.Enqueue(track);
                return (false, $"{track.Title} has been added to the queue.");
            }
            await player.PlayAsync(track);
            return (false, $"Now Playing: **{track.Title}**");
        }

        public async Task<string> StopAsync(ulong guildId)
        {
            var player = _lavaSocketClient.GetPlayer(guildId);
            if (player is null)
                return "Error with Player";
            await player.StopAsync();
            return "Music Playback Stopped.";
        }

        public async Task<string> SkipAsync(ulong guildId)
        {
            var player = _lavaSocketClient.GetPlayer(guildId);
            if (player is null || !player.Queue.Items.Any())
                return "Nothing in queue.";

            var oldTrack = player.CurrentTrack;
            await player.SkipAsync();
            return $"Skiped: **{oldTrack.Title}** \nNow Playing: **{player.CurrentTrack.Title}**";
        }

        public async Task<string> PauseOrResumeAsync(ulong guildId)
        {
            var player = _lavaSocketClient.GetPlayer(guildId);
            if (player is null)
                return "Player isn't playing.";

            if (!player.IsPaused)
            {
                await player.PauseAsync();
                return "Player is Paused.";
            }
            else
            {
                await player.ResumeAsync();
                return "Playback resumed.";
            }
        }

        public async Task<string> ResumeAsync(ulong guildId)
        {
            var player = _lavaSocketClient.GetPlayer(guildId);
            if (player is null)
                return "Player isn't playing.";

            if (player.IsPaused)
            {
                await player.ResumeAsync();
                return "Playback resumed.";
            }

            return "Player is not paused.";
        }

        public Task<string> Shuffle(ulong guildId)
        {
            var player = _lavaSocketClient.GetPlayer(guildId);
            if (player is null)
                return Task.FromResult("Player isn't playing.");
            player.Queue.Shuffle();
            return Task.FromResult("Player was shuffled");
        }

        private async Task ClientReadyAsync() =>
            await _lavaSocketClient.StartAsync(_client,
                new Configuration
                {
                    AutoDisconnect = true,
                    InactivityTimeout = TimeSpan.FromSeconds(30),
                    LogSeverity = LogSeverity.Verbose
                });

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

        private UriResult GetUriResult(string query) =>
            Uri.TryCreate(query, UriKind.Absolute, out var uriResult) &&
                (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
                ? uriResult.Host == "www.soundcloud.com"
                    ? UriResult.SoundCloud
                    : uriResult.Query.Split('=', '?', '&').Contains("list") ? UriResult.PlayList : UriResult.YoutubeLink
                : UriResult.NotUrl;

        private string MakeTrackList(string prefix, LavaTrack[] tracks)
        {
            var builder = new StringBuilder($"**Do `{prefix}play 1-5` to choose one of the tracks!**\n");
            for (var i = 0; i < tracks.Length && i < 5; i++)
                builder.Append($"**{i + 1}:** {tracks[i].Title} **({(tracks[i].Length.Hours > 0 ? tracks[i].Length.ToString(@"hh\:mm\:ss") : tracks[i].Length.ToString(@"mm\:ss"))})**\n");
            return builder.ToString();
        }

        private async Task<string> MakePlayList(LavaPlayer player, SearchResult results)
        {
            if (!results.Tracks.Any())
                return "**{results.PlaylistInfo.Name}** is empty!";

            var playListTracks = results.Tracks.ToArray();

            if (player.IsPlaying)
                player.Queue.Enqueue(playListTracks[0]);
            else await player.PlayAsync(playListTracks[0]);

            for (var i = 1; i < playListTracks.Length; i++)
                player.Queue.Enqueue(playListTracks[i]);

            return $"Loaded **{results.PlaylistInfo.Name}** which has **{playListTracks.Length}** tracks";
        }
    }
}