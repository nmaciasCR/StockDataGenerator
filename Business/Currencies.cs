using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDataGenerator.Business
{
    public class Currencies : Interfaces.ICurrencies
    {
        private Repositories.Model.Entities.TradeAlertContext _dbContext;
        private readonly Services.ICurrencyConverter _currencyConverter;
        private Interfaces.IWriter _writer;

        public Currencies(Repositories.Model.Entities.TradeAlertContext dbContext, Services.ICurrencyConverter currencyConverter, Interfaces.IWriter writer)
        {
            _dbContext = dbContext;
            _currencyConverter = currencyConverter;
            _writer = writer;
        }


        /// <summary>
        /// Retorna un lisado de Currencies
        /// </summary>
        /// <returns></returns>
        public List<Repositories.Model.Entities.Currencies> GetList()
        {
            try
            {
                return _dbContext.Currencies.ToList();
            } catch
            {
                _writer.writeLine.Error("ERROR GetList(): No se puede obtener el listado de la tabla Currencies");
                return new List<Repositories.Model.Entities.Currencies>();
            }
        }

        /// <summary>
        /// Actualiza las cotizaciones de las monedas
        /// </summary>
        /// <returns></returns>
        public Boolean RefreshFromService()
        {
            Model.CurrencyConverterResponse currenciesResponse;
            DateTime dateToRefresh = DateTime.Now;
            try
            {
                List<Repositories.Model.Entities.Currencies> currenciesList = GetList();
                //respuesta del servicio de cotizaciones de monedas
                currenciesResponse = _currencyConverter.GetCurrencyExchange(currenciesList.Select(c => c.code.ToUpper()).ToList());
                //actualizamos la lista de monedas
                foreach (var c in currenciesResponse.rates)
                {
                    var currency = currenciesList.Find(cu => cu.code.ToUpper() == c.Key);
                    currency.updateDate = dateToRefresh;
                    currency.euroExchange = Math.Round(1 / c.Value, 3) ;
                }
                //Actualizamos las divisas en la DDBB
                return UpdateList(currenciesList);

            } catch
            {
                _writer.writeLine.Error("ERROR RefreshFromService(): No se puede obtener el listado de la tabla Currencies");
                return false;
            }
        }


        /// <summary>
        /// Actualizamos una lista de Currencies en la base de datos
        /// </summary>
        /// <param name="currenciesToUpdate"></param>
        /// <returns></returns>
        public bool UpdateList(List<Repositories.Model.Entities.Currencies> currenciesToUpdate)
        {
            try
            {
                _dbContext.Currencies.UpdateRange(currenciesToUpdate);
                _dbContext.SaveChanges();
                return true;
            } catch
            {
                _writer.writeLine.Error("ERROR currenciesToUpdate()");
                return false;
            }
        }


    }
}
