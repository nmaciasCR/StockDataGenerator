using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDataGenerator.Business.Model
{
    public class YahooFinanceQuotes
    {
        public string currency { get; set; }
        public string market { get; set; }
        public string marketState { get; set; }
        public double regularMarketPrice { get; set; }
        public double regularMarketChangePercent { get; set; }
        public string symbol { get; set; }


    }


}
