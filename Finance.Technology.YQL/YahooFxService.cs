using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Finance.Services.Financial;
using Finance.Technology.Entity;
using log4net;

namespace Finance.Technology.Yahoo
{
    public class YahooFxService : IFxService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (YahooFxService));

        private const string BaseQueryUrl =
            "http://finance.yahoo.com/webservice/v1/symbols/allcurrencies/quote?format=xml";


        public IEnumerable<Currency> FetchAll()
        {
            Log.Debug(string.Format("Fetching info from {0} yql url", BaseQueryUrl));

            var yahooResponse = XDocument.Load(BaseQueryUrl);

            return Parse(yahooResponse);
        }

        private static IEnumerable<Currency> Parse(XDocument yahooResponse)
        {
            var results = yahooResponse.Root.Element("resources");

            return results.Elements("resource").Select(NewCurrency);
        }

        private static Currency NewCurrency(XElement resourceXml)
        {
            return new Currency
                       {
                           Symbol = resourceXml.Elements("field").First(x => x.Attribute("name").Value == "symbol").Value,
                           Name = resourceXml.Elements("field").First(x => x.Attribute("name").Value == "name").Value,
                           Type = resourceXml.Elements("field").First(x => x.Attribute("name").Value == "type").Value,
                           Price = resourceXml.Elements("field").First(x => x.Attribute("name").Value == "price").Value.GetDecimal(),
                           UtcTime = DateTime.Parse(resourceXml.Elements("field").First(x => x.Attribute("name").Value == "utctime").Value),
                           LastUpdate = DateTime.Now,
                           SourceRepresentation = resourceXml.ToString()
                       };
        }
    }
}