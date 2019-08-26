using DestDiscordBotV3.Data;
using DestDiscordBotV3.Model;
using DestDiscordBotV3.Service.Interface;
using Discord.Commands;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DestDiscordBotV3.Service.External
{
    [Group("Todo")]
    public class TodoService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<Todo> _todo;
        private readonly ITodoFactory _todoFactory;

        public TodoService(IRepository<Todo> todo, ITodoFactory todoFactory)
        {
            _todo = todo ?? throw new ArgumentNullException(nameof(todo));
            _todoFactory = todoFactory ?? throw new ArgumentNullException(nameof(todoFactory));
        }

        [Command, Priority(0)]
        public async Task Default() =>
            await ReplyAsync($"**{Context.User.Username}**, the correct usage is: `{Context.Prefix}todo list | clear | remove <number> | add <text>`");

        [Command("list"), Priority(1)]
        public async Task List()
        {
            var list = await _todo.GetAllByExpression(f => f.UserId == Context.User.Id);
            var builder = new StringBuilder($":notepad_spiral: **{Context.User.Username}**'s To-Do list!\n");
            for (var i = 0; i < list.Count; i++)
                builder.Append($"**[{i + 1}]** {list[i]}\n");
            await ReplyAsync(builder.ToString());
        }

        [Command("clear"), Priority(1)]
        public async Task Clear()
        {
            await _todo.DeleteMany(f => f.UserId == Context.User.Id);
            await ReplyAsync("Your To-Do list is now empty!");
        }

        [Command("remove"), Priority(1)]
        public async Task Remove(int pos)
        {
            var list = await _todo.GetAllByExpression(f => f.UserId == Context.User.Id);
            await _todo.Delete(list[pos - 1].Id);
            await ReplyAsync("Removed a to-do from your To-Do list!");
        }

        [Command("add"), Priority(1)]
        public async Task Add([Remainder] string text)
        {
            await _todo.Create(_todoFactory.Create(Context.User.Id, text));
            await ReplyAsync("Added a to-do to your To-Do list!");
        }
    }
}