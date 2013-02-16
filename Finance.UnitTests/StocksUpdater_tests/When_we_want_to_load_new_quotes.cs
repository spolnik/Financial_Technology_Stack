using System.Collections.Generic;
using Finance.DataAccess;
using Finance.Services;
using Finance.Services.Financial;
using Finance.Services.General;
using Finance.Technology.Entity;
using Finance.Technology.Services.StocksDownloader.Service;
using Finance.UnitTests.Framework;
using Moq;
using NUnit.Framework;

namespace Finance.UnitTests.StocksUpdater_tests
{
    [Category("Stocks update")]
    public class When_we_want_to_load_new_quotes : InstanceSpecification<IService>
    {
        private Mock<IStockService> _stockEngineMock;
        private Mock<IRepository<Quote>> _repositoryMock;

        protected override void Establish_context()
        {
            var msft = new Quote {Symbol = "MSFT"};
            var aapl = new Quote {Symbol = "AAPL"};

            _repositoryMock = new Mock<IRepository<Quote>>();
            _repositoryMock.Setup(x => x.Save(msft)).Verifiable();
            _repositoryMock.Setup(x => x.Save(aapl)).Verifiable();
            
            _stockEngineMock = new Mock<IStockService>();
            _stockEngineMock.Setup(x => x.Fetch(new List<string> {"MSFT", "AAPL"})).Returns(new List<Quote> {msft, aapl})
                .Verifiable();
        }

        protected override IService Create_subject_under_test()
        {
            return new StocksDownloadService(_repositoryMock.Object, _stockEngineMock.Object);
        }

        protected override void Because()
        {
            SubjectUnderTest.Process();
        }

        [Test]
        public void It_should_save_all_quotes_mentioned_in_stock_symbols_file()
        {
            _repositoryMock.VerifyAll();
            _stockEngineMock.VerifyAll();
        }
    }
}