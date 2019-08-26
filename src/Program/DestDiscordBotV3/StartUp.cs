using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DestDiscordBotV3
{
    public static class StartUp
    {
        public static string GetToken() =>
            File.ReadAllText("Resources/BotToken.txt");

        public static async Task DoChecks(DInjection dInjection)
        {
            // Check Bot Token
            if (!File.Exists("Resources/BotToken.txt"))
                throw new IOException("File BotToken.txt not found");
            //Check 8ball answers
            var eightBall = dInjection.Resolve<IRepository<EightBall>>();
            await CheckEightBall(eightBall);
            // Check Cat Facts
            var catFacts = dInjection.Resolve<IRepository<CatFact>>();
            await CheckCatFacts(catFacts);
            // Check Dog Facts
            var dogFacts = dInjection.Resolve<IRepository<DogFact>>();
            await CheckDogFacts(dogFacts);
            // Check Fortunes
            var fortunes = dInjection.Resolve<IRepository<Fortune>>();
            await CheckFortunes(fortunes);
        }

        private static async Task CheckCatFacts(IRepository<CatFact> catFact)
        {
            if (!File.Exists("Resources/CatFacts.txt"))
                return;
            var list = await File.ReadAllLinesAsync("Resources/CatFacts.txt");
            foreach (var item in list)
            {
                var result = await catFact.GetByExpression(f => f.Msg == item);
                if (result == null)
                    await catFact.Create(new CatFact
                    {
                        Id = Guid.NewGuid(),
                        Msg = item
                    });
            }
        }

        private static async Task CheckEightBall(IRepository<EightBall> eightBall)
        {
            if (!File.Exists("Resources/8ballAnswers.txt"))
                return;
            var list = await File.ReadAllLinesAsync("Resources/8ballAnswers.txt");
            foreach (var item in list)
            {
                var result = await eightBall.GetByExpression(f => f.Msg == item);
                if (result == null)
                    await eightBall.Create(new EightBall
                    {
                        Id = Guid.NewGuid(),
                        Msg = item
                    });
            }
        }

        private static async Task CheckDogFacts(IRepository<DogFact> dogFact)
        {
            if (!File.Exists("Resources/DogFacts.txt"))
                return;
            var list = await File.ReadAllLinesAsync("Resources/DogFacts.txt");
            foreach (var item in list)
            {
                var result = await dogFact.GetByExpression(f => f.Msg == item);
                if (result == null)
                    await dogFact.Create(new DogFact
                    {
                        Id = Guid.NewGuid(),
                        Msg = item
                    });
            }
        }

        private static async Task CheckFortunes(IRepository<Fortune> eightBall)
        {
            if (!File.Exists("Resources/Fortunes.txt"))
                return;
            var list = await File.ReadAllLinesAsync("Resources/Fortunes.txt");
            foreach (var item in list)
            {
                var result = await eightBall.GetByExpression(f => f.Msg == item);
                if (result == null)
                    await eightBall.Create(new Fortune
                    {
                        Id = Guid.NewGuid(),
                        Msg = item
                    });
            }
        }
    }
}