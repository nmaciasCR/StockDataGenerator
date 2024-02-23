using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDataGenerator.Business
{
    public class Stocks : Interfaces.IStocks
    {
        //private readonly ILogger<Stocks> _logger;
        private Repositories.Model.Entities.TradeAlertContext _dbContext;
        private readonly Services.IYahooFinanceApi _servicesYahooFinance;
        private Business.Interfaces.IWriter _writer;
        private readonly Interfaces.IDateConverter _dateConverter;

        public Stocks(Repositories.Model.Entities.TradeAlertContext dbContext, Services.IYahooFinanceApi servicesYahooFinance,
            Business.Interfaces.IWriter writer, Interfaces.IDateConverter dateConverter)
        {
            //this._logger = logger;
            _dbContext = dbContext;
            _servicesYahooFinance = servicesYahooFinance;
            _writer = writer;
            _dateConverter = dateConverter;
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
        public async Task<Boolean> RefreshFromService()
        {
            List<Repositories.Model.Entities.Quotes> stockToUpdate;
            List<Model.YahooFinanceQuotes> yahooQuotes;
            List<Repositories.Model.Entities.Quotes> subProcessList = new List<Repositories.Model.Entities.Quotes>();
            DateTime dateToUpdate = DateTime.Now;
            int stockNumToProcess = 10;
            int iteraction = 0;

            try
            {
                stockToUpdate = GetList();
                //Indicamos cantidad de acciones
                _writer.write.Info("Se actualizaran ");
                _writer.write.Success(stockToUpdate.Count().ToString());
                _writer.write.Info("acciones");
                Console.WriteLine();


                //tomamos las primeras (stockNumToProcess) stocks
                subProcessList = stockToUpdate.Take(stockNumToProcess).ToList();

                while (subProcessList.Any())
                {
                    //Cotizaciones de yahoo
                    yahooQuotes = await _servicesYahooFinance.GetQuotesAsync(subProcessList);

                    //update
                    foreach (Model.YahooFinanceQuotes yq in yahooQuotes)
                    {
                        Repositories.Model.Entities.Quotes q = subProcessList.First(s => s.symbol == yq.symbol);
                        try
                        {
                            q.regularMarketPrice = Convert.ToDecimal(yq.regularMarketPrice);
                            q.regularMarketChangePercent = Convert.ToDecimal(yq.regularMarketChangePercent);
                            q.regularMarketChange = Convert.ToDecimal(yq.regularMarketChange);
                            q.updateDate = dateToUpdate;
                            q.timezoneName = yq.exchangeTimezoneName;
                            q.earningsDate = _dateConverter.TimestampToDatetime(yq.earningsTimestamp, yq.exchangeTimezoneName);
                            //Mostramos el resultado sin error
                            _writer.write.Info(q.symbol + ": ");
                            _writer.write.Success("OK!");
                            Console.WriteLine();
                        }
                        catch (Exception ex)
                        {
                            //Mostramos el resultado con error
                            _writer.write.Info(q.symbol + ": ");
                            _writer.write.Error("ERROR! -> " + JsonConvert.SerializeObject(yq) + " - Exception: " + ex.Message);
                            Console.WriteLine();
                        }
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
                _writer.writeLine.Error("ERROR: RefreshFromService - " + ex.Message);
                _writer.writeLine.Error("ERROR: " + string.Join(",", subProcessList.Select(q => q.symbol)));
                return false;
            }

        }


    }
}
