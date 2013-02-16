using System.Globalization;
using System.Threading;
using Castle.Windsor;
using Finance.DataAccess;
using Finance.Services.Financial;
using Finance.Technology.Entity;
using Finance.Technology.Services.StocksDownloader.Installers;
using Finance.Technology.Services.StocksDownloader.Service;
using Quartz;
using log4net;

namespace Finance.Technology.Services.StocksDownloader.Scheduling
{
    public class StocksDownloadJob : IJob
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (StocksDownloadJob));

        public void Execute(JobExecutionContext context)
        {
            Log.Info("Stocks Download Job - execution started");
            Log.Info("Changing culture to en-US - yahoo service");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            using (var container = BootstrapContainer())
            {
                var repository = container.Resolve<IRepository<Quote>>();
                var stockService = container.Resolve<IStockService>();

                var service = new StocksDownloadService(repository, stockService);
                service.Process();
            }

            Log.Info("Stocks Download Job - execution finished");
        }

        private static IWindsorContainer BootstrapContainer()
        {
            return new WindsorContainer().Install(StocksDownloaderInstaller.CreateIt());
        }
    }
}