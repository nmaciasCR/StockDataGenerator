using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace StockDataGenerator
{
    public class Generator
    {
        private readonly IConfiguration _configuration;
        //private readonly ILogger<Generator> _logger;
        private readonly Business.Interfaces.IStocks _businessStock;
        private readonly Business.Interfaces.IWriter _writer;
        private readonly Business.Interfaces.ICurrencies _businessCurrencies;

        public async Task RunAsync()
        {
            //_logger.LogInformation("Comienza StockDataGenerator");
            _writer.writeLine.Info("Comienza StockDataGenerator");
            _writer.writeLine.Info("---------------------------------");
            _writer.writeLine.Info("");
            _writer.writeLine.Info("Actualizando cotizaciones de acciones...");
            _writer.writeLine.Info("");
            var serviceStockResult = await _businessStock.RefreshFromService();
            _writer.writeLine.Info("");
            _writer.writeLine.Info("Actualizando cotizaciones de divisas...");
            _writer.writeLine.Info("");
            var serviceCurrencyResult = await _businessCurrencies.RefreshFromService();
            //_logger.LogInformation("Fin StockDataGenerator");
            _writer.writeLine.Info("");
            _writer.writeLine.Info("Fin StockDataGenerator");
            _writer.writeLine.Info("---------------------------------");
            _writer.writeLine.Info("");
        }


        public Generator(IConfiguration configuration, Business.Interfaces.IStocks businessStock, Business.Interfaces.IWriter writer,
            Business.Interfaces.ICurrencies currencies )
        {
            this._configuration = configuration;
            //this._logger = logger;
            this._businessStock = businessStock;
            this._businessCurrencies = currencies;
            this._writer = writer;
        }
    }
}