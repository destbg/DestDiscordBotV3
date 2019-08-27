using Autofac;
using Autofac.Extensions.DependencyInjection;
using DestDiscordBotV3.Common.Guild;
using DestDiscordBotV3.Common.Logging;
using DestDiscordBotV3.Common.Score;
using DestDiscordBotV3.Data;
using DestDiscordBotV3.Service.Internal;
using Discord.Commands;
using Discord.WebSocket;
using MongoDB.Driver;
using System;

namespace DestDiscordBotV3
{
    public class DInjection
    {
        private readonly IContainer _container;

        public DInjection()
        {
            var builder = new ContainerBuilder();
            var client = new DiscordSocketClient();

            builder.RegisterInstance(client);

            // Register Types
            builder.RegisterType<CommandService>();
            builder.RegisterType<CommandHandler>().As<ICommandHandler>();
            builder.RegisterType<Connection>().As<IConnection>();

            var db = new MongoClient().GetDatabase("DestDiscordBotV3");
            builder.RegisterInstance(db);
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerDependency();

            builder.RegisterAssemblyTypes(typeof(UserFactory).Assembly,
                typeof(DiscordLogger).Assembly,
                typeof(GuildPrefix).Assembly)
                .AsImplementedInterfaces();

            //Register Provider
            builder.RegisterInstance(CreateProvider(db));

            _container = builder.Build();
        }

        public T Resolve<T>() =>
            _container.Resolve<T>();

        private IServiceProvider CreateProvider(IMongoDatabase db)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterInstance(db);
            containerBuilder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerDependency();
            //Register Factories
            containerBuilder.RegisterAssemblyTypes(typeof(ReportFactory).Assembly)
                .AsImplementedInterfaces();
            return new AutofacServiceProvider(containerBuilder.Build());
        }
    }
}