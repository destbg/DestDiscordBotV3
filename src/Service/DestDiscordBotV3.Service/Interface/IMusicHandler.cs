using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.Interface
{
    public interface IMusicHandler
    {
        Task ConnectAsync(SocketVoiceChannel voiceChannel, ITextChannel textChannel);
        Task Initialize();
        Task LeaveAsync(SocketVoiceChannel voiceChannel);
        Task<string> PauseOrResumeAsync(ulong guildId);
        Task<(bool, string)> PlayAsync(string query, ulong guildId, string prefix, int pos = -1);
        Task<string> ResumeAsync(ulong guildId);
        Task<string> SetVolumeAsync(int vol, ulong guildId);
        Task<string> SkipAsync(ulong guildId);
        Task<string> StopAsync(ulong guildId);
    }
}