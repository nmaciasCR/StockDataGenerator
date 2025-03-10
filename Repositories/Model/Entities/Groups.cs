using System;
using System.Collections.Generic;

#nullable disable

namespace StockDataGenerator.Repositories.Model.Entities
{
    public partial class Groups
    {
        public Groups()
        {
            QuotesGroups = new HashSet<QuotesGroups>();
        }

        public int ID { get; set; }
        public string description { get; set; }

        public virtual ICollection<QuotesGroups> QuotesGroups { get; set; }
    }
}
