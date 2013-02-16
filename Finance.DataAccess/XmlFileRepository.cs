using System.IO;
using System.Text;
using Finance.Common.Serialization;
using Finance.Technology.Entity;
using log4net;

namespace Finance.DataAccess
{
    public class XmlFileRepository<TFinancialItem> : IRepository<TFinancialItem> where TFinancialItem : AFinancialEntity
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(XmlFileRepository<TFinancialItem>));

        public void Save(TFinancialItem item)
        {
            var fileName = new StringBuilder();
            fileName.Append(item.Symbol).Append(item.LastUpdate.ToString("_yyyy_MM_dd_HH_mm_ss")).Append(".xml");

            var financialItemName = typeof (TFinancialItem).Name;

            if (!Directory.Exists(financialItemName))
                new DirectoryInfo(financialItemName).Create();

            var filePath = Path.Combine(financialItemName, fileName.ToString());
            _log.Info(string.Format("Saving {0} ({1}) to location: {2}", financialItemName, item.Symbol, filePath));

            XmlSerializationHelper<TFinancialItem>.SerializeToFile(item, filePath);
        }
    }
}