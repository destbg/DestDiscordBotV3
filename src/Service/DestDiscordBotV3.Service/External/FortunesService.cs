using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    [Group("fortune")]
    public class FortunesService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<Fortune> _fortune;

        public FortunesService(IRepository<Fortune> fortune)
        {
            _fortune = fortune ?? throw new ArgumentNullException(nameof(fortune));
        }

        [Command]
        public async Task FortuneAsync()
        {
            var list = await _fortune.GetAllToList();
            await ReplyAsync($"**:crystal_ball: {list[new Random().Next(list.Count)]}**");
        }
    }
}