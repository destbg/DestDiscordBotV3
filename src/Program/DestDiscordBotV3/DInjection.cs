namespace DestDiscordBotV3
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Common.Guild;
    using Common.Logging;
    using Common.Redstone;
    using Common.Score;
    using Data;
    using DestDiscordBotV3.Common.Math;
    using DestDiscordBotV3.Common.Music;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using MongoDB.Driver;
    using Service.Internal;
    using System;
    using System.IO;
    using Victoria;

    public class DInjection : IDInjection
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
                    LogLevel = LogSeverity.Verbose
                });
            var service = new CommandService(
                new CommandServiceConfig
                {
                    LogLevel = LogSeverity.Verbose,
                    CaseSensitiveCommands = false
                });

            // Check Log
            if (!Directory.Exists("Log"))
                Directory.CreateDirectory("Log");

            var writer = new StreamWriter($"Log/{DateTime.UtcNow.ToString().Replace(':', '.')}.txt")
            {
                AutoFlush = true
            };

            builder.RegisterInstance(client).SingleInstance();
            builder.RegisterInstance(service).SingleInstance();
            builder.RegisterInstance(writer).SingleInstance();

            // Register Types
            builder.RegisterType<Logger>().As<ILogger>();
            builder.RegisterType<DiscordLogger>().As<IDiscordLogger>();
            builder.RegisterType<CommandHandler>().As<ICommandHandler>();
            builder.RegisterType<Connection>().As<IConnection>();

            var db = new MongoClient().GetDatabase("DestDiscordBotV3");
            builder.RegisterInstance(db).SingleInstance();
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerDependency();

            builder.RegisterAssemblyTypes(typeof(UserFactory).Assembly,
                typeof(DiscordLogger).Assembly,
                typeof(GuildHandler).Assembly,
                typeof(Repeater).Assembly)
                .AsImplementedInterfaces();

            //Register Provider
            builder.RegisterInstance(CreateProvider(db, client, writer));

            _container = builder.Build();
        }

        public T Resolve<T>() =>
            _container.Resolve<T>();

        private IServiceProvider CreateProvider(IMongoDatabase db, DiscordSocketClient client, StreamWriter writer)
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(typeof(ReportFactory).Assembly)
                .AsImplementedInterfaces();

            // For Music Integration
            builder.RegisterInstance(writer).SingleInstance();
            builder.RegisterInstance(client).SingleInstance();
            builder.RegisterType<Logger>().As<ILogger>();
            builder.RegisterType<DiscordLogger>().As<IDiscordLogger>();
            builder.RegisterType<LavaRestClient>();
            builder.RegisterType<LavaSocketClient>().SingleInstance();
            builder.RegisterType<MusicHandler>().As<IMusicHandler>();
            builder.RegisterType<MathHandler>().As<IMathHandler>();

            builder.RegisterType<Random>();
            builder.RegisterInstance(db).SingleInstance();
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>))
                .InstancePerDependency();

            return new AutofacServiceProvider(builder.Build());
        }
    }
}