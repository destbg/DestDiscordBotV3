namespace DestDiscordBotV3.Service.External
{
    using Data;
    using Data.Extension;
    using Discord.Commands;
    using Model;
    using Service.Interface;
    using System;
    using System.Text;
    using System.Threading.Tasks;

    [Group("todo")]
    public class TodoService : ModuleBase<CommandContextWithPrefix>
    {
        private readonly IRepository<Todo> _todo;
        private readonly ITodoFactory _todoFactory;

        public TodoService(IRepository<Todo> todo, ITodoFactory todoFactory)
        {
            _todo = todo ?? throw new ArgumentNullException(nameof(todo));
            _todoFactory = todoFactory ?? throw new ArgumentNullException(nameof(todoFactory));
        }

        [Command]
        public async Task List()
        {
            var list = await _todo.GetAllByCondition(f => f.UserId == Context.User.Id);
            if (list.Count == 0)
            {
                await ReplyAsync("Your to-do list is empty");
                return;
            }
            var builder = new StringBuilder($":notepad_spiral: **{Context.User.Username}**'s To-Do list!\n");
            for (var i = 0; i < list.Count; i++)
                builder.Append($"**[{i + 1}]** {list[i].Msg}\n");
            await ReplyAsync(builder.ToString());
        }

        [Command("clear")]
        public async Task Clear()
        {
            await _todo.DeleteMany(f => f.UserId == Context.User.Id);
            await ReplyAsync("Your To-Do list is now empty!");
        }

        [Command("remove")]
        public async Task Remove(int pos)
        {
            var list = await _todo.GetAllByCondition(f => f.UserId == Context.User.Id);
            if (pos < 1 || pos > list.Count)
            {
                await ReplyAsync("There is no to-do at that number");
                return;
            }
            await _todo.Delete(list[pos - 1].Id);
            await ReplyAsync("Removed a to-do from your To-Do list!");
        }

        [Command("add")]
        public async Task Add([Remainder] string text)
        {
            if (text.Length > 200)
            {
                await ReplyAsync("The length of the text can be up to 200 characters");
                return;
            }
            var count = await _todo.GetAll().Where(f => f.UserId == Context.User.Id).CountDocumentsAsync();
            if (count >= 10)
            {
                await ReplyAsync("You can only have 10 to-do on your list");
                return;
            }
            await _todo.Create(_todoFactory.Create(Context.User.Id, text));
            await ReplyAsync("Added a to-do to your list!");
        }
    }
}