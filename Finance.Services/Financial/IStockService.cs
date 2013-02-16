using System.Collections.Generic;
using Finance.Technology.Entity;

namespace Finance.Services.Financial
{
    public interface IStockService
    {
        IEnumerable<Quote> Fetch(IList<string> symbols);
    }
}