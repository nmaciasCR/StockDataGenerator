using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace StockDataGenerator
{
    public class Generator
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Generator> _logger;
        private readonly Business.Interfaces.IStocks _businessStock;
        private readonly Business.Interfaces.IWriter _writer;

        public void Run()
        {
            //_logger.LogInformation("Comienza StockDataGenerator");
            _writer.writeLine.Info("Comienza StockDataGenerator");
            _writer.writeLine.Info("---------------------------------");
            _writer.writeLine.Info("");
            _businessStock.RefreshFromService();
            //_logger.LogInformation("Fin StockDataGenerator");
            _writer.writeLine.Info("Fin StockDataGenerator");
            _writer.writeLine.Info("---------------------------------");
            _writer.writeLine.Info("");
        }


        public Generator(IConfiguration configuration, ILogger<Generator> logger, Business.Interfaces.IStocks businessStock, Business.Interfaces.IWriter writer)
        {
            this._configuration = configuration;
            this._logger = logger;
            this._businessStock = businessStock;
            this._writer = writer;
        }
    }
}