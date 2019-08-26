using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    [Group("catfacts")]
    public class CatFactsService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<CatFact> _catFact;

        public CatFactsService(IRepository<CatFact> catFact)
        {
            _catFact = catFact ?? throw new ArgumentNullException(nameof(catFact));
        }

        [Command]
        public async Task CatFactsAsync()
        {
            var list = await _catFact.GetAllToList();
            await ReplyAsync($"**:cat: {list[new Random().Next(list.Count)]}**");
        }
    }
}