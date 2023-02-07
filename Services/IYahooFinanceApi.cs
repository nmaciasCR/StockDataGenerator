using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDataGenerator.Services
{
    public interface IYahooFinanceApi
    {
        List<Business.Model.YahooFinanceQuotes> GetQuotes(List<Repositories.Model.Entities.Quotes> quotesList);
    }
}
