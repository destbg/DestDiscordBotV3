using DestDiscordBotV3.Model;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using org.mariuszgromada.math.mxparser;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    public class AllService : ModuleBase<CommandContextWithPrefix>
    {
        [Command("ping")]
        public async Task Ping()
        {
            var msg = await ReplyAsync("Pong!");
            await msg.ModifyAsync(m => m.Content = $"Pong! Time taken - **{(msg.Timestamp - DateTimeOffset.Now).Milliseconds}ms**");
        }

        [Command("math")]
        public async Task Math([Remainder] string expression) =>
            await ReplyAsync(new Expression(expression).calculate().ToString());

        [Command("wiki")]
        public async Task Wiki([Remainder] string text)
        {
            var search = string.Join("+", text.Split(" ".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries));
            await ReplyAsync(":book: **https://en.wikipedia.org/wiki/" + search + "**");
        }

        [Command("google")]
        public async Task Google([Remainder] string text)
        {
            var search = string.Join("+", text.Split(" ".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries));
            await ReplyAsync(":eye_in_speech_bubble: **https://www.google.com/search?q=" + search + "&oq=" + search + "**");
        }

        [Command("randomPerson"), Alias("rP", "getRandomPerson")]
        public async Task RandomPerson()
        {
            var json = "";
            using (var client = new WebClient())
                json = client.DownloadString("https://randomuser.me/api/");
            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);
            string firstName = dataObject.results[0].name.first.ToString();
            string lastName = dataObject.results[0].name.last.ToString();
            string avatarURL = dataObject.results[0].picture.large.ToString();
            var embed = new EmbedBuilder();
            embed.WithThumbnailUrl(avatarURL);
            embed.WithTitle("Generated person");
            embed.AddField("First name", firstName);
            embed.AddField("Last name", lastName);
            embed.Color = new Color(102, 255, 153);
            await ReplyAsync("", embed: embed.Build());
        }

        [Command("reverse")]
        public async Task Reverse([Remainder] string text)
        {
            var charArray = text.ToCharArray();
            Array.Reverse(charArray);
            await ReplyAsync("**Reversed text:** " + new string(charArray));
        }

        [Command("time")]
        public async Task Time() =>
            await ReplyAsync($"The Utc time is: **{DateTime.UtcNow.TimeOfDay}**");
    }
}