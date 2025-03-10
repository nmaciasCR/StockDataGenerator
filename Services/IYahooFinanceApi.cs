using StockDataGenerator.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDataGenerator.Services
{
    public interface IYahooFinanceApi
    {
        Task<List<YahooFinanceQuotes>> GetQuotesAsync(List<Repositories.Model.Entities.Quotes> quotesList);
        Task<List<YahooFinanceSpark>> GetSparkAsync(List<Repositories.Model.Entities.Quotes> quotesList);
    }
}
