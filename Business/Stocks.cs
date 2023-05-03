using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDataGenerator.Business
{
    public class Stocks : Interfaces.IStocks
    {
        private readonly ILogger<Stocks> _logger;
        private Repositories.Model.Entities.TradeAlertContext _dbContext;
        private readonly Services.IYahooFinanceApi _servicesYahooFinance;
        private Business.Interfaces.IPriority _priority;

        public Stocks(ILogger<Stocks> logger, Repositories.Model.Entities.TradeAlertContext dbContext, Services.IYahooFinanceApi servicesYahooFinance,
            Business.Interfaces.IPriority businessPriority)
        {
            this._logger = logger;
            this._dbContext = dbContext;
            this._servicesYahooFinance = servicesYahooFinance;
            this._priority = businessPriority;
        }


        /// <summary>
        /// Retorna un listado de las acciones
        /// </summary>
        public List<Repositories.Model.Entities.Quotes> GetList()
        {
            List<Repositories.Model.Entities.Quotes> list = new List<Repositories.Model.Entities.Quotes>();

            try
            {
                list = _dbContext.Quotes
                    .Include(q => q.market)
                    .Include(q => q.priority)
                    .Include(q => q.QuotesAlerts)
                    .ToList();

                return list;
            }
            catch (Exception ex)
            {
                return list;
            }

        }


        /// <summary>
        /// Actualizamos las cotizaciones desde YahooFinance
        /// </summary>
        /// <returns></returns>
        public Boolean RefreshFromService()
        {
            List<Repositories.Model.Entities.Quotes> stockToUpdate;
            List<Model.YahooFinanceQuotes> yahooQuotes;
            DateTime dateToUpdate = DateTime.Now;
            int stockNumToProcess = 10;
            int iteraction = 0;

            try
            {
                stockToUpdate = GetList();

                //tomamos las primeras (stockNumToProcess) stocks
                List<Repositories.Model.Entities.Quotes> subProcessList = stockToUpdate.Take(stockNumToProcess).ToList();

                while (subProcessList.Any())
                {
                    //Cotizaciones de yahoo
                    yahooQuotes = this._servicesYahooFinance.GetQuotes(subProcessList);

                    //update
                    foreach (Model.YahooFinanceQuotes yq in yahooQuotes)
                    {
                        Repositories.Model.Entities.Quotes q = subProcessList.First(s => s.symbol == yq.symbol);
                        q.regularMarketPrice = Convert.ToDecimal(yq.regularMarketPrice);
                        q.regularMarketChangePercent = Convert.ToDecimal(yq.regularMarketChangePercent);
                        q.updateDate = dateToUpdate;
                        //q.priorityId = this._priority.DefinePriority(q.regularMarketPrice, q.QuotesAlerts.Select(q => q.price).ToList());
                        q.regularMarketChange = Convert.ToDecimal(yq.regularMarketChange);
                    }

                    //Actualizamos en la DB
                    _dbContext.Quotes.UpdateRange(subProcessList);
                    _dbContext.SaveChanges();

                    iteraction++;
                    subProcessList = stockToUpdate.Skip(stockNumToProcess * iteraction).Take(stockNumToProcess).ToList();
                }

                return true;

            }
            catch (Exception ex)
            {
                ConsoleColor colorDefault = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = colorDefault;

                return false;
            }

        }










    }
}
