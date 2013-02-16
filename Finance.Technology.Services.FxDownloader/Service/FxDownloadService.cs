using Finance.Common.Extension;
using Finance.DataAccess;
using Finance.Services.Financial;
using Finance.Services.General;
using Finance.Technology.Entity;
using log4net;

namespace Finance.Technology.Services.FxDownloader.Service
{
    public class FxDownloadService : IService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (FxDownloadService));

        private readonly IRepository<Currency> _repository;
        private readonly IFxService _fxService;

        public FxDownloadService(IRepository<Currency> repository, IFxService fxService)
        {
            _repository = repository;
            _fxService = fxService;
        }

        public void Process()
        {
            Log.Info("FxDownloadService is processing...");

            var currencies = _fxService.FetchAll();
            currencies.ForEach(_repository.Save);
        }
    }
}