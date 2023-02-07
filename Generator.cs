using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace StockDataGenerator
{
    public class Generator
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Generator> _logger;
        private readonly Business.Interfaces.IStocks _businessStock;

        public void Run()
        {
            _logger.LogInformation("Comienza StockDataGenerator");
            _businessStock.RefreshFromService();
            _logger.LogInformation("Fin StockDataGenerator");
        }


        public Generator(IConfiguration configuration, ILogger<Generator> logger, Business.Interfaces.IStocks businessStock)
        {
            this._configuration = configuration;
            this._logger = logger;
            this._businessStock = businessStock;
        }
    }
}