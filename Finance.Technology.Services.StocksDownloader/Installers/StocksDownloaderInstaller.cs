using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Finance.DataAccess;
using Finance.Services.Financial;
using Finance.Technology.Entity;
using Finance.Technology.Yahoo;

namespace Finance.Technology.Services.StocksDownloader.Installers
{
    public class StocksDownloaderInstaller : IWindsorInstaller
    {
        private StocksDownloaderInstaller()
        {
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IRepository<Quote>>()
                    .ImplementedBy<XmlFileRepository<Quote>>(),
                Component.For<IStockService>()
                    .ImplementedBy<YahooStockService>());
        }

        public static IWindsorInstaller CreateIt()
        {
            return new StocksDownloaderInstaller();
        }
    }
}