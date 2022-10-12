using System;
using System.Reflection.Emit;
using NLog;
using NLog.Targets;

namespace ETWParser
{
    public static class Logger
    {
        public static void InitLogger(string loggerName, LogLevel level)
        {
            ColoredConsoleTarget target = new ColoredConsoleTarget();
            target.Layout = "${date:format=HH\\:MM\\:ss} ${logger} ${message}";

            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, level);

            logger = NLog.LogManager.GetLogger(loggerName);
        }

        public static NLog.Logger logger;
    }
}