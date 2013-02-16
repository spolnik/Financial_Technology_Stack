using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Finance.DataAccess;
using Finance.Services.Financial;
using Finance.Technology.Entity;
using Finance.Technology.Yahoo;

namespace Finance.Technology.Services.FxDownloader.Installers
{
    internal class FxDownloaderInstaller : IWindsorInstaller
    {
        private FxDownloaderInstaller()
        {
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IRepository<Currency>>()
                    .ImplementedBy<XmlFileRepository<Currency>>(),
                Component.For<IFxService>()
                    .ImplementedBy<YahooFxService>());
        }

        public static IWindsorInstaller CreateIt()
        {
            return new FxDownloaderInstaller();
        }
    }
}
