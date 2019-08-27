using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    [Group("8ball")]
    public class EightBallService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<EightBall> _eightBall;

        public EightBallService(IRepository<EightBall> eightBall)
        {
            _eightBall = eightBall ?? throw new ArgumentNullException(nameof(eightBall));
        }

        [Command]
        public async Task EightBallAsync([Remainder] string question)
        {
            var list = await _eightBall.GetAllToList();
            await ReplyAsync($":8ball: **Question:** {question}\n**Answer:** {list[new Random().Next(list.Count)].Msg}");
        }
    }
}