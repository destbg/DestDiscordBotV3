using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    public class AppResourceService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<AppResource> _resource;

        public AppResourceService(IRepository<AppResource> resource)
        {
            _resource = resource ?? throw new ArgumentNullException(nameof(resource));
        }

        [Command("catfacts")]
        public async Task CatFactsAsync()
        {
            var list = await _resource.GetAllByExpression(f => f.ResourceType == ResourceType.CatFact);
            await ReplyAsync($"**:cat: {list[new Random().Next(list.Count)].Msg}**");
        }

        [Command("dogfacts")]
        public async Task DogFactsAsync()
        {
            var list = await _resource.GetAllByExpression(f => f.ResourceType == ResourceType.DogFact);
            await ReplyAsync($"**:dog: {list[new Random().Next(list.Count)].Msg}**");
        }

        [Command("8ball")]
        public async Task EightBallAsync([Remainder] string question)
        {
            var list = await _resource.GetAllByExpression(f => f.ResourceType == ResourceType.EightBall);
            await ReplyAsync($":8ball: **Question:** {question}\n**Answer:** {list[new Random().Next(list.Count)].Msg}");
        }

        [Command("fortune")]
        public async Task FortuneAsync()
        {
            var list = await _resource.GetAllByExpression(f => f.ResourceType == ResourceType.Fortune);
            await ReplyAsync($"**:crystal_ball: {list[new Random().Next(list.Count)].Msg}**");
        }
    }
}