using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.Interface
{
    public interface IMusicHandler
    {
        Task Initialize();

        Task UserVoiceStateUpdated(SocketUser user, SocketVoiceState botVoice, SocketVoiceState userVoice);

        Task LeaveAsync(SocketVoiceChannel voiceChannel);

        Task<string> PauseOrResumeAsync(ulong guildId);

        Task<(bool, string)> PlayAsync(SocketVoiceChannel voiceChannel, ITextChannel textChannel, string query, ulong guildId, string prefix, int pos = -1);

        Task<string> ResumeAsync(ulong guildId);

        Task<string> Shuffle(ulong guildId);

        Task<string> SkipAsync(ulong guildId);

        Task<string> StopAsync(ulong guildId);
    }
}