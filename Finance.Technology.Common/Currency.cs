using System;

namespace Finance.Technology.Entity
{
    public class Currency : AFinancialEntity
    {
        public decimal? Price { get; set; }
        public string Type { get; set; }
        public DateTime UtcTime { get; set; }
    }
}