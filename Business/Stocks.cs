using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StockDataGenerator.Repositories.Model;

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
            List<Model.YahooFinanceSpark> yahooSpark;
            List<Repositories.Model.Entities.Quotes> subProcessList = new List<Repositories.Model.Entities.Quotes>();
            DateTime dateToUpdate = DateTime.Now;
            int stockNumToProcess = 10;
            int iteraction = 0;

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

                try
                {
                    //Cotizaciones de yahoo
                    yahooQuotes = await _servicesYahooFinance.GetQuotesAsync(subProcessList);
                    //Pausa de medio segundo
                    Thread.Sleep(500);
                    //Historico de valores de cierre
                    yahooSpark = await _servicesYahooFinance.GetSparkAsync(subProcessList);


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
                            q.isCached = false;

                            //Calculamos el estocastico %K y %D
                            Model.YahooFinanceSpark quoteSpark = yahooSpark.FirstOrDefault(s => s.symbol == yq.symbol);
                            if (quoteSpark != null && quoteSpark.close != null)
                            {
                                //ordenamos los cierres de mas reciente a menos reciente
                                quoteSpark.close.Reverse();
                                //estocastico % K
                                q.stochasticK = GetstochasticK(q.symbol, quoteSpark.close);
                                //estocastico % D
                                q.stochasticD = GetstochasticD(q.symbol, quoteSpark.close);
                            }

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
                    //Pausa de 1 segundo
                    Thread.Sleep(1000);

                }
                catch (Exception ex)
                {
                    _writer.writeLine.Error("ERROR: RefreshFromService - " + ex.Message);
                    _writer.writeLine.Error("ERROR: " + string.Join(",", subProcessList.Select(q => q.symbol)));
                    //return false;
                }

                iteraction++;
                subProcessList = stockToUpdate.Skip(stockNumToProcess * iteraction).Take(stockNumToProcess).ToList();
            }

            return true;
        }




        /// <summary>
        /// Calcula el estocastico %K de una accion
        /// </summary>
        /// <param name="price"></param>
        /// <param name="closePriceList"></param>
        /// <returns></returns>
        private double GetstochasticK(string symbol, List<double> closePriceList)
        {

            try
            {
                //Solo necesitamos los ultimos 14 cierres
                List<double> closePrices = closePriceList.Take(14).ToList();
                //Ultimos precio
                double price = closePrices.First();
                //Minimo cierre
                double minPrice = closePrices.Min();
                //Maximos precio
                double maxPrice = closePrices.Max();

                //Calculamos para obtener el estocastico K
                double stochasticK = ((price - minPrice) / (maxPrice - minPrice)) * 100;

                return stochasticK;


            } catch (Exception ex)
            {
                _writer.write.Error($"ERROR! -> GetstochasticK({symbol}) - Exception: " + ex.Message);
                return 0;
            }

        }

        /// <summary>
        /// Calculo Stocastico %D (ULTIMOS 3 PERIODOS %K)
        /// </summary>
        /// <param name="closePriceList"></param>
        /// <returns></returns>
        private double GetstochasticD(string symbol, List<double> closePriceList)
        {
            double stochasticToday = -1;
            double stochasticYesterday = -1;
            double stochasticBeforeYesterday = -1;

            try
            {
                //Los estocasticos de los ultimos 3 dias
                stochasticToday = GetstochasticK(symbol, closePriceList);
                stochasticYesterday = GetstochasticK(symbol, closePriceList.Skip(1).ToList());
                stochasticBeforeYesterday = GetstochasticK(symbol, closePriceList.Skip(2).ToList());

                return (stochasticToday + stochasticYesterday + stochasticBeforeYesterday) / 3;

            } catch (Exception ex)
            {
                _writer.write.Error($"ERROR! -> GetstochasticD({symbol}): {stochasticToday}, {stochasticYesterday}, {stochasticBeforeYesterday}  - Exception: " + ex.Message);
                return 0;
            }
        }


    }
}
