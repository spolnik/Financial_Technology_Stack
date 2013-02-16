using System.Collections.Generic;
using Finance.Technology.Entity;

namespace Finance.Services.Financial
{
    public interface IFxService
    {
        IEnumerable<Currency> FetchAll();
    }
}