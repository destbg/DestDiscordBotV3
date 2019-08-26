using Autofac;
using Autofac.Extensions.DependencyInjection;
using DestDiscordBotV3.Data;
using DestDiscordBotV3.Logger;
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

            // Register Types
            builder.RegisterType<DiscordSocketClient>();
            builder.RegisterType<Logging>().As<ILogging>();
            builder.RegisterType<DiscordLogger>().As<IDiscordLogger>();
            builder.RegisterType<CommandService>();
            builder.RegisterType<CommandHandler>().As<ICommandHandler>();
            builder.RegisterType<Connection>().As<IConnection>();

            var db = new MongoClient().GetDatabase("DestDiscordBotV3");
            builder.RegisterInstance(db);
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerDependency();

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
                .Where(w => w.Name.EndsWith("Factory"))
                .AsImplementedInterfaces();
            return new AutofacServiceProvider(containerBuilder.Build());
        }
    }
}