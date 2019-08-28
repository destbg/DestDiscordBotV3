using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
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
            for (int i = 0; i < list.Length; i++)
            {
                CatFact result;
                try
                {
                    result = await catFact.GetByExpression(f => f.Msg == list[i]);
                }
                catch
                {
                    result = null;
                }
                if (result == null)
                    await catFact.Create(new CatFact
                    {
                        Id = i + 1,
                        Msg = list[i]
                    });
            }
        }

        private static async Task CheckEightBall(IRepository<EightBall> eightBall)
        {
            if (!File.Exists("Resources/8ballAnswers.txt"))
                return;
            var list = await File.ReadAllLinesAsync("Resources/8ballAnswers.txt");
            for (int i = 0; i < list.Length; i++)
            {
                EightBall result;
                try
                {
                    result = await eightBall.GetByExpression(f => f.Msg == list[i]);
                }
                catch
                {
                    result = null;
                }
                if (result == null)
                    await eightBall.Create(new EightBall
                    {
                        Id = i + 1,
                        Msg = list[i]
                    });
            }
        }

        private static async Task CheckDogFacts(IRepository<DogFact> dogFact)
        {
            if (!File.Exists("Resources/DogFacts.txt"))
                return;
            var list = await File.ReadAllLinesAsync("Resources/DogFacts.txt");
            for (int i = 0; i < list.Length; i++)
            {
                DogFact result;
                try
                {
                    result = await dogFact.GetByExpression(f => f.Msg == list[i]);
                }
                catch
                {
                    result = null;
                }
                if (result == null)
                    await dogFact.Create(new DogFact
                    {
                        Id = i + 1,
                        Msg = list[i]
                    });
            }
        }

        private static async Task CheckFortunes(IRepository<Fortune> fortune)
        {
            if (!File.Exists("Resources/Fortunes.txt"))
                return;
            var list = await File.ReadAllLinesAsync("Resources/Fortunes.txt");
            for (int i = 0; i < list.Length; i++)
            {
                Fortune result;
                try
                {
                    result = await fortune.GetByExpression(f => f.Msg == list[i]);
                }
                catch
                {
                    result = null;
                }
                if (result == null)
                    await fortune.Create(new Fortune
                    {
                        Id = i + 1,
                        Msg = list[i]
                    });
            }
        }
    }
}