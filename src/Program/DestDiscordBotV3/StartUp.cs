using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DestDiscordBotV3
{
    public static class StartUp
    {
        public static string GetToken() =>
            File.ReadAllText("BotToken.txt");

        public static async Task DoChecks(DInjection dInjection)
        {
            // Check Bot Token
            if (!File.Exists("BotToken.txt"))
                throw new IOException("File BotToken.txt not found");

            // Check Resources
            var appResource = dInjection.Resolve<IRepository<AppResource>>();
            await DoResourcesFileCheck(appResource, "Resources/8ballAnswers.txt", ResourceType.EightBall, 0);
            await DoResourcesFileCheck(appResource, "Resources/CatFacts.txt", ResourceType.CatFact, 1000);
            await DoResourcesFileCheck(appResource, "Resources/DogFacts.txt", ResourceType.DogFact, 2000);
            await DoResourcesFileCheck(appResource, "Resources/Fortunes.txt", ResourceType.Fortune, 3000);

            // Check Help
            var appHelp = dInjection.Resolve<IRepository<AppHelp>>();
            await DoHelpCheck(appHelp, "Help/Core.json", HelpType.Core);
            await DoHelpCheck(appHelp, "Help/Fun.json", HelpType.Fun);
            await DoHelpCheck(appHelp, "Help/Music.json", HelpType.Music);
        }

        private static async Task DoResourcesFileCheck(IRepository<AppResource> repository, string file, ResourceType resourceType, int add)
        {
            if (!File.Exists(file))
                return;
            var list = await File.ReadAllLinesAsync(file);
            for (int i = 0; i < list.Length; i++)
            {
                AppResource result;
                try
                {
                    result = await repository.GetByExpression(f => f.Msg == list[i]);
                }
                catch
                {
                    result = null;
                }
                if (result is null)
                {
                    await repository.Create(new AppResource
                    {
                        Id = i + add + 1,
                        ResourceType = resourceType,
                        Msg = list[i]
                    });
                }
            }
        }

        private static async Task DoHelpCheck(IRepository<AppHelp> repository, string file, HelpType helpType)
        {
            if (!File.Exists(file))
                return;
            var list = JsonConvert.DeserializeObject<IReadOnlyList<AppHelp>>(File.ReadAllText(file));
            for (int i = 0; i < list.Count; i++)
            {
                AppHelp result;
                try
                {
                    result = await repository.GetById(list[i].Id);
                }
                catch
                {
                    result = null;
                }
                if (result is null)
                {
                    list[i].HelpType = helpType;
                    await repository.Create(list[i]);
                }
            }
        }
    }
}