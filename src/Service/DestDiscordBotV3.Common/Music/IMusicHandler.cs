namespace DestDiscordBotV3.Common.Music
{
    using Discord;
    using Discord.WebSocket;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IMusicHandler" />
    /// </summary>
    public interface IMusicHandler
    {
        /// <summary>
        /// Initialize the <see cref="MusicHandler"/>
        /// </summary>
        Task Initialize();

        /// <summary>
        /// Leave the voice channel
        /// </summary>
        Task LeaveAsync(SocketVoiceChannel voiceChannel);

        /// <summary>
        /// Pause Or Resume playing
        /// </summary>
        Task<string> PauseOrResumeAsync(ulong guildId);

        /// <summary>
        /// Join the voice channel and play music
        /// </summary>
        Task<(bool, string)> PlayAsync(SocketVoiceChannel voiceChannel, ITextChannel textChannel, string query, ulong guildId, string prefix, int pos = -1);

        /// <summary>
        /// Resume playing the music
        /// </summary>
        Task<string> ResumeAsync(ulong guildId);

        /// <summary>
        /// Shuffle the play-list
        /// </summary>
        Task<string> Shuffle(ulong guildId);

        /// <summary>
        /// Skip a song and go to the next one
        /// </summary>
        Task<string> SkipAsync(ulong guildId);

        /// <summary>
        /// Stop playing music
        /// </summary>
        Task<string> StopAsync(ulong guildId);

        /// <summary>
        /// User left or joined a voice channel
        /// </summary>
        Task UserVoiceStateUpdated(SocketUser user, SocketVoiceState lastVoice, SocketVoiceState newVoice);
    }
}