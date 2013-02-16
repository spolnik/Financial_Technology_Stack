using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Finance.Common.Extension;
using Finance.Common.Validation;
using Finance.Services.Financial;
using Finance.Technology.Entity;
using log4net;

namespace Finance.Technology.Yahoo
{
    public class YahooStockService : IStockService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (YahooStockService));

        private const string YqlBaseQueryUrl =
            "http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20({0})&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";

        public IEnumerable<Quote> Fetch(IList<string> symbols)
        {
            Guard.IsNotNull(symbols, "symbols");

            var symbolList = String.Join("%2C", symbols.Select(x => string.Format("%22{0}%22", x)).ToArray());
            var url = string.Format(YqlBaseQueryUrl, symbolList);

            Log.Debug(string.Format("Fetching info from {0} yql url", url));

            var yqlResponse = XDocument.Load(url);

            return Parse(symbols, yqlResponse);
        }

        private static IEnumerable<Quote> Parse(IEnumerable<string> symbols, XDocument yqlResponse)
        {
            var results = yqlResponse.Root.Element("results");

            return symbols
                .Select(symbol => results.Elements("quote").First(x => x.Attribute("symbol").Value == symbol))
                .Select(NewQuote);
        }

        private static Quote NewQuote(XElement quoteXml)
        {
            var newQuote = new Quote
                               {
                                   Symbol = quoteXml.Attribute("symbol").Value,
                                   Ask = StringConverter.GetDecimal(quoteXml.Element("Ask").Value),
                                   Bid = StringConverter.GetDecimal(quoteXml.Element("Bid").Value),
                                   AverageDailyVolume = StringConverter.GetDecimal(quoteXml.Element("AverageDailyVolume").Value),
                                   BookValue = StringConverter.GetDecimal(quoteXml.Element("BookValue").Value),
                                   Change = StringConverter.GetDecimal(quoteXml.Element("Change").Value),
                                   DividendShare = StringConverter.GetDecimal(quoteXml.Element("DividendShare").Value),
                                   LastTradeDate =
                                       StringConverter.GetDateTime(string.Format("{0} {1}", quoteXml.Element("LastTradeDate").Value,
                                                                 quoteXml.Element("LastTradeTime").Value)),
                                   EarningsShare = StringConverter.GetDecimal(quoteXml.Element("EarningsShare").Value),
                                   EpsEstimateCurrentYear = StringConverter.GetDecimal(quoteXml.Element("EPSEstimateCurrentYear").Value),
                                   EpsEstimateNextYear = StringConverter.GetDecimal(quoteXml.Element("EPSEstimateNextYear").Value),
                                   EpsEstimateNextQuarter = StringConverter.GetDecimal(quoteXml.Element("EPSEstimateNextQuarter").Value),
                                   DailyLow = StringConverter.GetDecimal(quoteXml.Element("DaysLow").Value),
                                   DailyHigh = StringConverter.GetDecimal(quoteXml.Element("DaysHigh").Value),
                                   YearlyLow = StringConverter.GetDecimal(quoteXml.Element("YearLow").Value),
                                   YearlyHigh = StringConverter.GetDecimal(quoteXml.Element("YearHigh").Value),
                                   MarketCapitalization = StringConverter.GetDecimal(quoteXml.Element("MarketCapitalization").Value),
                                   Ebitda = StringConverter.GetDecimal(quoteXml.Element("EBITDA").Value),
                                   ChangeFromYearLow = StringConverter.GetDecimal(quoteXml.Element("ChangeFromYearLow").Value),
                                   PercentChangeFromYearLow =
                                       StringConverter.GetDecimal(quoteXml.Element("PercentChangeFromYearLow").Value),
                                   ChangeFromYearHigh = StringConverter.GetDecimal(quoteXml.Element("ChangeFromYearHigh").Value),
                                   LastTradePrice = StringConverter.GetDecimal(quoteXml.Element("LastTradePriceOnly").Value),
                                   PercentChangeFromYearHigh =
                                       StringConverter.GetDecimal(quoteXml.Element("PercebtChangeFromYearHigh").Value),
                                   FiftyDayMovingAverage = StringConverter.GetDecimal(quoteXml.Element("FiftydayMovingAverage").Value),
                                   TwoHunderedDayMovingAverage =
                                       StringConverter.GetDecimal(quoteXml.Element("TwoHundreddayMovingAverage").Value),
                                   ChangeFromTwoHundredDayMovingAverage =
                                       StringConverter.GetDecimal(quoteXml.Element("ChangeFromTwoHundreddayMovingAverage").Value),
                                   PercentChangeFromTwoHundredDayMovingAverage =
                                       StringConverter.GetDecimal(quoteXml.Element("PercentChangeFromTwoHundreddayMovingAverage").Value),
                                   PercentChangeFromFiftyDayMovingAverage =
                                       StringConverter.GetDecimal(quoteXml.Element("PercentChangeFromFiftydayMovingAverage").Value),
                                   Name = quoteXml.Element("Name").Value,
                                   Open = StringConverter.GetDecimal(quoteXml.Element("Open").Value),
                                   PreviousClose = StringConverter.GetDecimal(quoteXml.Element("PreviousClose").Value),
                                   ChangeInPercent = StringConverter.GetDecimal(quoteXml.Element("ChangeinPercent").Value),
                                   PriceSales = StringConverter.GetDecimal(quoteXml.Element("PriceSales").Value),
                                   PriceBook = StringConverter.GetDecimal(quoteXml.Element("PriceBook").Value),
                                   ExDividendDate = StringConverter.GetDateTime(quoteXml.Element("ExDividendDate").Value),
                                   PeRatio = StringConverter.GetDecimal(quoteXml.Element("PERatio").Value),
                                   DividendPayDate = StringConverter.GetDateTime(quoteXml.Element("DividendPayDate").Value),
                                   PegRatio = StringConverter.GetDecimal(quoteXml.Element("PEGRatio").Value),
                                   PriceEpsEstimateCurrentYear =
                                       StringConverter.GetDecimal(quoteXml.Element("PriceEPSEstimateCurrentYear").Value),
                                   PriceEpsEstimateNextYear =
                                       StringConverter.GetDecimal(quoteXml.Element("PriceEPSEstimateNextYear").Value),
                                   ShortRatio = StringConverter.GetDecimal(quoteXml.Element("ShortRatio").Value),
                                   OneYearPriceTarget = StringConverter.GetDecimal(quoteXml.Element("OneyrTargetPrice").Value),
                                   Volume = StringConverter.GetDecimal(quoteXml.Element("Volume").Value),
                                   StockExchange = quoteXml.Element("StockExchange").Value,
                                   LastUpdate = DateTime.Now,
                                   SourceRepresentation = quoteXml.ToString()
                               };


            return newQuote;
        }
    }

    internal static class StringConverter
    {
        internal static decimal? GetDecimal(this string input)
        {
            if (input.IsNull())
                return null;

            input = input.Replace("%", String.Empty);

            decimal value;

            if (Decimal.TryParse(input, out value))
                return value;

            return null;
        }

        internal static DateTime? GetDateTime(string input)
        {
            if (input.IsNull())
                return null;

            DateTime value;

            if (DateTime.TryParse(input, out value))
                return value;

            return null;
        }

        internal static long ToLong(this string input)
        {
            return Convert.ToInt64(input);
        }
    }
}
