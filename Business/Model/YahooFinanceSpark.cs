using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDataGenerator.Business.Model
{
    public class YahooFinanceSpark
    {
        public List<long> timestamp { get; set; }
        public string symbol { get; set; }
        public int dataGranularity { get; set; }
        public double? previousClose { get; set; }
        public double chartPreviousClose { get; set; }
        public List<double> close { get; set; }
    }
}
