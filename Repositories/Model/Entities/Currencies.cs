using System;
using System.Collections.Generic;

#nullable disable

namespace StockDataGenerator.Repositories.Model.Entities
{
    public partial class Currencies
    {
        public Currencies()
        {
            Quotes = new HashSet<Quotes>();
        }

        public int ID { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public double euroExchange { get; set; }
        public DateTime? updateDate { get; set; }

        public virtual ICollection<Quotes> Quotes { get; set; }
    }
}
