using System;
using System.IO;
using log4net;
using log4net.Config;

namespace Finance.Services.General
{
    public static class Logging
    {
        public static void BootstrapLogger(this ILog log)
        {
            var configurationFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");

            var configurationFile = new FileInfo(configurationFilePath);

            if (configurationFile.Exists)
                XmlConfigurator.ConfigureAndWatch(configurationFile);
            else
                BasicConfigurator.Configure();

            log.DebugFormat("Logging configuration loaded: {0}", configurationFilePath);
        }
    }
}