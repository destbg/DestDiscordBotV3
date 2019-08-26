using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    public class DogFactsService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<DogFact> _dogFact;

        public DogFactsService(IRepository<DogFact> dogFact)
        {
            _dogFact = dogFact ?? throw new ArgumentNullException(nameof(dogFact));
        }

        [Command("dogfacts")]
        public async Task DogFactsAsync()
        {
            var list = await _dogFact.GetAllToList();
            await ReplyAsync($"**:dog: {list[new Random().Next(list.Count)]}**");
        }
    }
}