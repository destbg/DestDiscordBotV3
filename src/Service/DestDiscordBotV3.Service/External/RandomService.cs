namespace DestDiscordBotV3.Service.External
{
    using Discord.Commands;
    using Model;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class RandomService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly Random _random;

        public RandomService(Random random)
        {
            _random = random ?? throw new ArgumentNullException(nameof(random));
        }

        [Command("choose")]
        public async Task ChooseAsync([Remainder] string text)
        {
            var input = text.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries);
            if (input.Length == 1)
            {
                await ReplyAsync("You need to give at least 2 options");
                return;
            }
            await ReplyAsync($"**{Context.User.Username}**, i choose **{input[_random.Next(input.Length)]}**!");
        }

        [Command("coin")]
        public async Task Coin() =>
            await ReplyAsync("Your coin landed on **" +
                $"{new string[] { "HEADS", "TAILS" }[_random.Next(2)]}**!");

        [Command("dice")]
        public async Task Dice(int num) =>
            await ReplyAsync($"Your dice rolled **{_random.Next(num)}**!");

        [Command("rps"), Alias("rockPaperScissors")]
        public async Task RockPaperScissors(string cmd)
        {
            cmd = cmd.ToLower();
            var chosen = new[] { "rock", "paper", "scissors" }[_random.Next(3)];
            if (cmd == chosen)
            {
                await ReplyAsync($"__destbot__ chose **{chosen.ToUpper()}**, it's a tie!");
                return;
            }
            var arr = new[] { chosen, cmd };
            if (arr.Contains("rock") && arr.Contains("paper"))
                await ReplyAsync($"__destbot__ chose **{chosen.ToUpper()}**, **PAPER** wins!");
            else if (arr.Contains("scissors") && arr.Contains("paper"))
                await ReplyAsync($"__destbot__ chose **{chosen.ToUpper()}**, **SCISSORS** wins!");
            else if (arr.Contains("rock") && arr.Contains("scissors"))
                await ReplyAsync($"__destbot__ chose **{chosen.ToUpper()}**, **ROCK** wins!");
            else await ReplyAsync("You can only choose between rock, paper or scissors");
        }
    }
}