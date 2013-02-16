using System.Reflection;
using Finance.Services;
using Finance.Services.General;
using Finance.Services.Scheduling;
using Finance.Technology.Services.StocksDownloader.Scheduling;
using log4net;

namespace Finance.Technology.Services.StocksDownloader
{
    public class StocksDownloadRunner
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (StocksDownloadRunner));

        public static void Main(string[] args)
        {
            Log.BootstrapLogger();

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var serviceName = string.Format("FinanceTechnology_StocksDownloader_v{0}", version);

            var host = ServiceHostFactory.New<Scheduler<StocksDownloadJob>>(serviceName, Log);

            host.Run();
        }
    }
}