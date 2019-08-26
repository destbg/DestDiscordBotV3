using DestDiscordBotV3.Model;

namespace DestDiscordBotV3.Service.Interface
{
    public interface ITagFactory
    {
        Tag Create(ulong userId, string tag, string message);
    }
}