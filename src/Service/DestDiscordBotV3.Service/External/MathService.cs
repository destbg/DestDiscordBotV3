namespace DestDiscordBotV3.Service.External
{
    using Common.Math;
    using Discord.Commands;
    using Model;
    using System;
    using System.Threading.Tasks;

    [Group("math")]
    public class MathService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IMathHandler _math;

        public MathService(IMathHandler math)
        {
            _math = math ?? throw new ArgumentNullException(nameof(math));
        }

        [Command]
        public async Task Math([Remainder] string expression) =>
            await ReplyAsync(_math.Calculate(expression));
    }
}