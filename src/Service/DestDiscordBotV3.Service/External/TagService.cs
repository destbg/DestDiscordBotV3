using DestDiscordBotV3.Data;
using DestDiscordBotV3.Data.Extension;
using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.Interface;
using Discord;
using Discord.Commands;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    [Group("tag")]
    public class TagService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<Tag> _tag;
        private readonly ITagFactory _tagFactory;

        public TagService(IRepository<Tag> tag, ITagFactory tagFactory)
        {
            _tag = tag ?? throw new ArgumentNullException(nameof(tag));
            _tagFactory = tagFactory ?? throw new ArgumentNullException(nameof(tagFactory));
        }

        [Command, Priority(0)]
        public async Task Default(string tag)
        {
            var msg = await _tag.GetByExpression(f => f.UserId == Context.User.Id && f.Name == tag);
            if (msg is null)
                return;
            await ReplyAsync(msg.Msg);
        }

        [Command("create"), Priority(1)]
        public async Task Create(string tag, [Remainder] string msg)
        {
            if (msg.Length > 200)
            {
                await ReplyAsync("The length of the message can be up to 200 characters");
                return;
            }
            var count = await _tag.GetAll().Where(f => f.UserId == Context.User.Id && f.Name == tag).CountDocumentsAsync();
            if (count >= 10)
            {
                await ReplyAsync("You can only have 10 tags");
                return;
            }
            await _tag.Create(_tagFactory.Create(Context.User.Id, tag, msg));
            await ReplyAsync($"Created tag **{tag}** successfully!");
        }

        [Command("change"), Priority(1)]
        public async Task Change(string tag, [Remainder] string msg)
        {
            var result = await _tag.GetByExpression(f => f.UserId == Context.User.Id && f.Name == tag);
            if (result is null)
            {
                await ReplyAsync($"You don't have a tag **{tag}**");
                return;
            }
            result.Msg = msg;
            await _tag.Update(result, result.Id);
            await ReplyAsync($"Changed tag **{tag}** successfully!");
        }

        [Command("remove"), Priority(1)]
        public async Task Remove(string tag)
        {
            var result = await _tag.GetByExpression(f => f.UserId == Context.User.Id && f.Name == tag);
            if (result is null)
            {
                await ReplyAsync($"You don't have a tag **{tag}**");
                return;
            }
            await _tag.Delete(result.Id);
            await ReplyAsync($"Removed tag **{tag}** successfully!");
        }

        [Command("list"), Priority(1)]
        public async Task List(IUser user = null)
        {
            var target = user ?? Context.User;
            var list = await _tag.GetAllByExpression(f => f.UserId == target.Id);
            if (list.Count == 0)
            {
                await ReplyAsync($"**{target.Username}** doesn't have any tags");
                return;
            }
            var builder = new StringBuilder($":notepad_spiral: List of tags for **{target.Username}**.\n");
            foreach (var item in list)
                builder.Append($"**[{item.Name}]** {item.Msg}\n");
            await ReplyAsync(builder.ToString());
        }
    }
}