using System;
using System.Collections.Generic;

#nullable disable

namespace StockDataGenerator.Repositories.Model.Entities
{
    public partial class QuotesAlertsTypes
    {
        public QuotesAlertsTypes()
        {
            QuotesAlerts = new HashSet<QuotesAlerts>();
        }

        public int ID { get; set; }
        public string description { get; set; }
        public string message { get; set; }

        public virtual ICollection<QuotesAlerts> QuotesAlerts { get; set; }
    }
}
