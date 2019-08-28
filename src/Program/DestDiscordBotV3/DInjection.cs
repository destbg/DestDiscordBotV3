using Autofac;
using Autofac.Extensions.DependencyInjection;
using DestDiscordBotV3.Common.Guild;
using DestDiscordBotV3.Common.Logging;
using DestDiscordBotV3.Common.Score;
using DestDiscordBotV3.Data;
using DestDiscordBotV3.Service.Extension;
using DestDiscordBotV3.Service.Interface;
using DestDiscordBotV3.Service.Internal;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MongoDB.Driver;
using System;
using Victoria;

namespace DestDiscordBotV3
{
    public class DInjection
    {
        private readonly IContainer _container;

        public DInjection()
        {
            var builder = new ContainerBuilder();
            var client = new DiscordSocketClient(
                new DiscordSocketConfig
                {
                    AlwaysDownloadUsers = true,
                    MessageCacheSize = 50,
                    LogLevel = LogSeverity.Debug
                });
            var service = new CommandService(
                new CommandServiceConfig
                {
                    LogLevel = LogSeverity.Debug,
                    CaseSensitiveCommands = false
                });

            builder.RegisterInstance(client).SingleInstance();
            builder.RegisterInstance(service).SingleInstance();

            // Register Types
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
            builder.RegisterInstance(CreateProvider(db, client));

            _container = builder.Build();
        }

        public T Resolve<T>() =>
            _container.Resolve<T>();

        private IServiceProvider CreateProvider(IMongoDatabase db, DiscordSocketClient client)
        {
            var builder = new ContainerBuilder();

            // For Music Integration
            builder.RegisterInstance(client).SingleInstance();
            builder.RegisterType<LavaRestClient>();
            builder.RegisterType<LavaSocketClient>().SingleInstance();
            builder.RegisterType<MusicHandler>()
                .As<IMusicHandler>();

            builder.RegisterInstance(db);
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerDependency();

            // Register Factories
            builder.RegisterAssemblyTypes(typeof(ReportFactory).Assembly)
                .AsImplementedInterfaces();
            return new AutofacServiceProvider(builder.Build());
        }
    }
}