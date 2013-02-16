using System.Collections.Generic;
using System.IO;
using Finance.Common.Extension;
using Finance.DataAccess;
using Finance.Services;
using Finance.Services.Financial;
using Finance.Services.General;
using Finance.Technology.Entity;
using log4net;

namespace Finance.Technology.Services.StocksDownloader.Service
{
    public class StocksDownloadService : IService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (StocksDownloadService));

        private const string StockSymbolsTxt = "stock_symbols.txt";

        private readonly IRepository<Quote> _repository;
        private readonly IStockService _stockService;

        public StocksDownloadService(IRepository<Quote> repository, IStockService stockService)
        {
            _repository = repository;
            _stockService = stockService;
        }

        public void Process()
        {
            Log.Info("StocksDownloadService is processing...");

            var symbols = LoadSymbols();
            var quotes = _stockService.Fetch(symbols);

            quotes.ForEach(_repository.Save);
        }

        private static IList<string> LoadSymbols()
        {
            var fileInfo = new FileInfo(StockSymbolsTxt);
            Log.Info(string.Format("Loading stock symbols from: {0}", fileInfo.FullName));

            var symbolsToReturn = new List<string>();

            using (var reader = fileInfo.OpenText())
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var symbols = line.Split(new[] {';'});

                    symbolsToReturn.AddRange(symbols);
                }
            }

            Log.Info(string.Format("Loaded {0} symbols", symbolsToReturn.Count));

            return symbolsToReturn;
        }
    }
}
