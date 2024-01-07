using System;
using System.Collections.Generic;

#nullable disable

namespace StockDataGenerator.Repositories.Model.Entities
{
    public partial class Quotes
    {
        public Quotes()
        {
            QuotesAlerts = new HashSet<QuotesAlerts>();
        }

        public int ID { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public int marketId { get; set; }
        public int currencyId { get; set; }
        public decimal regularMarketPrice { get; set; }
        public decimal? regularMarketChangePercent { get; set; }
        public DateTime? updateDate { get; set; }
        public int priorityId { get; set; }
        public decimal? regularMarketChange { get; set; }
        public DateTime dateReview { get; set; }
        public bool isMainIndex { get; set; }

        public virtual Currencies currency { get; set; }
        public virtual Markets market { get; set; }
        public virtual QuotesPriority priority { get; set; }
        public virtual Portfolio Portfolio { get; set; }
        public virtual ICollection<QuotesAlerts> QuotesAlerts { get; set; }
    }
}
