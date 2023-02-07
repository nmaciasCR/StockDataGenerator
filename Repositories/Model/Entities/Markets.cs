using System;
using System.Collections.Generic;

#nullable disable

namespace StockDataGenerator.Repositories.Model.Entities
{
    public partial class Markets
    {
        public Markets()
        {
            Quotes = new HashSet<Quotes>();
        }

        public int ID { get; set; }
        public string name { get; set; }
        public string state { get; set; }

        public virtual ICollection<Quotes> Quotes { get; set; }
    }
}
