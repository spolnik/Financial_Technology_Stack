using System.Reflection;
using Finance.Services.General;
using Finance.Services.Scheduling;
using Finance.Technology.Services.FxDownloader.Scheduling;
using log4net;

namespace Finance.Technology.Services.FxDownloader
{
    public class FxDownloadRunner
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (FxDownloadRunner));

        public static void Main(string[] args)
        {
            Log.BootstrapLogger();
            
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var serviceName = string.Format("FinanceTechnology_FxDownloader_v{0}", version);

            var host = ServiceHostFactory.New<Scheduler<FxDownloadJob>>(serviceName, Log);

            host.Run();
        }
    }
}
