using Topshelf;
using log4net;

namespace Finance.Services.General
{
    public static class ServiceHostFactory
    {
         public static Host New<TService>(string serviceName, ILog log) where TService : class, IStartableService, new()
         {
             return HostFactory.New(x =>
                                        {
                                            x.AfterInstall(
                                                () => log.Info(string.Format("{0} Service is installed", serviceName)));
                                            x.AfterUninstall(
                                                () => log.Info(string.Format("{0} Service is uninstalled", serviceName)));
                                            x.AfterStartingServices(
                                                () => log.Info(string.Format("{0} Service is started", serviceName)));
                                            x.AfterStoppingServices(
                                                () => log.Info(string.Format("{0} Service is stopped", serviceName)));

                                            x.EnableDashboard();

                                            x.Service<TService>(s =>
                                                                    {

                                                                        s.SetServiceName(serviceName);

                                                                        s.ConstructUsing(
                                                                            name => new TService());
                                                                        s.WhenStarted(tc => tc.Start());
                                                                        s.WhenStopped(tc => tc.Stop());
                                                                    });
                                            x.RunAsLocalSystem();

                                            x.SetDescription("Finance Technology - Service");
                                            x.SetDisplayName(serviceName);
                                            x.SetServiceName(serviceName);
                                        });
         }
    }
}