using System;
using System.Collections.Generic;

#nullable disable

namespace StockDataGenerator.Repositories.Model.Entities
{
    public partial class Portfolio
    {
        public int quoteId { get; set; }
        public int quantity { get; set; }

        public virtual Quotes quote { get; set; }
    }
}
