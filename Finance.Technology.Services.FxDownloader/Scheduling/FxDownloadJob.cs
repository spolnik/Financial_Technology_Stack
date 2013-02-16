using System.Globalization;
using System.Threading;
using Castle.Windsor;
using Finance.DataAccess;
using Finance.Services.Financial;
using Finance.Technology.Entity;
using Finance.Technology.Services.FxDownloader.Installers;
using Finance.Technology.Services.FxDownloader.Service;
using Quartz;
using log4net;

namespace Finance.Technology.Services.FxDownloader.Scheduling
{
    public class FxDownloadJob : IJob
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (FxDownloadJob));

        public void Execute(JobExecutionContext context)
        {
            Log.Info("Fx Download Job - execution started");
            Log.Info("Changing culture to en-US - yahoo service");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            using (var container = BootstrapContainer())
            {
                var repository = container.Resolve<IRepository<Currency>>();
                var fxService = container.Resolve<IFxService>();

                var service = new FxDownloadService(repository, fxService);
                service.Process();
            }

            Log.Info("Fx Download Job - execution finished");
        }

        private static IWindsorContainer BootstrapContainer()
        {
            return new WindsorContainer().Install(FxDownloaderInstaller.CreateIt());
        }
    }
}