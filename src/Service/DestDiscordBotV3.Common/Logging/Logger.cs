using System.IO;
using System;

namespace DestDiscordBotV3.Common.Logging
{
    public class Logger : ILogger
    {
        private readonly StreamWriter _writer;

        public Logger(StreamWriter writer)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
            _writer.WriteLine(message);
        }
    }
}