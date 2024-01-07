using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDataGenerator.Services
{
    public interface ICurrencyConverter
    {
        Business.Model.CurrencyConverterResponse GetCurrencyExchange(List<string> currencies);
    }
}
