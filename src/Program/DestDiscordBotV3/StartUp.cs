﻿namespace DestDiscordBotV3
{
    using Data;
    using Model;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// <see cref="StartUp" /> class
    /// </summary>
    public static class StartUp
    {
        /// <summary>
        /// Get the bot token from the BotToken.txt file
        /// </summary>
        public static string GetToken() =>
            File.ReadAllText("BotToken.txt");

        /// <summary>
        /// Do Folder Checks
        /// </summary>
        public static Task DoFolderChecks()
        {
            // Check Bot Token
            if (!File.Exists("BotToken.txt"))
                throw new IOException("File BotToken.txt not found");

            // Check Log
            if (!Directory.Exists("Log"))
                Directory.CreateDirectory("Log");

            return Task.CompletedTask;
        }

        /// <summary>
        /// Do start up checks
        /// </summary>
        public static async Task DoChecks(IDInjection dInjection)
        {
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

            // Check Admin Help
            var appAdminHelp = dInjection.Resolve<IRepository<AppAdminHelp>>();
            await DoAdminHelpCheck(appAdminHelp, "Help/Admin.json");
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
                    result = await repository.GetByCondition(f => f.Msg == list[i]);
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

        private static async Task DoAdminHelpCheck(IRepository<AppAdminHelp> repository, string file)
        {
            if (!File.Exists(file))
                return;
            var list = JsonConvert.DeserializeObject<IReadOnlyList<AppAdminHelp>>(File.ReadAllText(file));
            for (int i = 0; i < list.Count; i++)
            {
                AppAdminHelp result;
                try
                {
                    result = await repository.GetById(list[i].Id);
                }
                catch
                {
                    result = null;
                }
                if (result is null)
                    await repository.Create(list[i]);
            }
        }
    }
}
