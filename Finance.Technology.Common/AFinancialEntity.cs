using System;

namespace Finance.Technology.Entity
{
    public abstract class AFinancialEntity
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public DateTime LastUpdate { get; set; }
        public string SourceRepresentation { get; set; }
    }
}