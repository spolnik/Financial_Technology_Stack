using System.Collections.Generic;
using Finance.Services.Financial;
using Finance.Technology.Entity;
using Finance.Technology.Services.StocksDownloader.Service;
using Finance.Technology.Yahoo;
using Finance.UnitTests.Framework;
using NUnit.Framework;
using System.Linq;

namespace Finance.UnitTests.YQL_tests
{
    [Category("YQL")]
    public class When_we_want_to_fetch_msft_and_aapl_symbols_quotes_from_yahoo_finance : InstanceSpecification<IStockService>
    {
        private IEnumerable<Quote> _quotes;
        private List<string> _symbols;

        protected override void Establish_context()
        {
            _symbols = new List<string> {"MSFT", "AAPL"};
        }

        protected override IStockService Create_subject_under_test()
        {
            return new YahooStockService();
        }

        protected override void Because()
        {
            _quotes = SubjectUnderTest.Fetch(_symbols);
        }

        [Test]
        public void It_should_return_two_quotes()
        {
            var count = _quotes.Count();
            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void It_should_return_quote_with_msft_symbol()
        {
            var msft = _quotes.Select(x => x.Symbol.ToLower() == "msft");

            Assert.That(msft, Is.Not.Null);
        }

        [Test]
        public void It_should_return_quote_with_aapl_symbol()
        {
            var aapl = _quotes.Select(x => x.Symbol.ToLower() == "aapl");

            Assert.That(aapl, Is.Not.Null);
        }
    }
}