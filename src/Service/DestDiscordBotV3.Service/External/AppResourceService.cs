namespace DestDiscordBotV3.Service.External
{
    using Data;
    using Discord.Commands;
    using Model;
    using System;
    using System.Threading.Tasks;

    public class AppResourceService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<AppResource> _resource;
        private readonly Random _random;

        public AppResourceService(IRepository<AppResource> resource, Random random)
        {
            _resource = resource ?? throw new ArgumentNullException(nameof(resource));
            _random = random ?? throw new ArgumentNullException(nameof(random));
        }

        [Command("catfacts")]
        public async Task CatFacts()
        {
            var list = await _resource.GetAllByCondition(f => f.ResourceType == ResourceType.CatFact);
            await ReplyAsync($"**:cat: {list[_random.Next(list.Count)].Msg}**");
        }

        [Command("dogfacts")]
        public async Task DogFacts()
        {
            var list = await _resource.GetAllByCondition(f => f.ResourceType == ResourceType.DogFact);
            await ReplyAsync($"**:dog: {list[_random.Next(list.Count)].Msg}**");
        }

        [Command("8ball")]
        public async Task EightBall([Remainder] string question)
        {
            var list = await _resource.GetAllByCondition(f => f.ResourceType == ResourceType.EightBall);
            await ReplyAsync($":8ball: **Question:** {question}\n**Answer:** {list[_random.Next(list.Count)].Msg}");
        }

        [Command("fortune")]
        public async Task Fortune()
        {
            var list = await _resource.GetAllByCondition(f => f.ResourceType == ResourceType.Fortune);
            await ReplyAsync($"**:crystal_ball: {list[_random.Next(list.Count)].Msg}**");
        }
    }
}