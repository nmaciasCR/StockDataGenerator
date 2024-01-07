using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace StockDataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var generator = ActivatorUtilities.CreateInstance<Generator>(host.Services);
            generator.Run();
        }

        public static IHostBuilder CreateHostBuilder(String[] args)

        {
            var builder = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);
            IConfiguration _configuration = builder.Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, configuration) =>
                {
                    configuration.Sources.Clear();
                    configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<Repositories.Model.Entities.TradeAlertContext>(options => {
                        options.UseSqlServer(_configuration.GetConnectionString("TradeAlert"));
                    });
                    services.AddTransient<Business.Interfaces.IStocks, Business.Stocks>();
                    services.AddTransient<Services.IYahooFinanceApi, Services.YahooFinanceApi>();
                    services.AddSingleton<Business.Interfaces.IWriter, Utils.Writer>();
                    services.AddTransient<Services.ICurrencyConverter, Services.CurrencyConverter>();
                    services.AddTransient<Business.Interfaces.ICurrencies, Business.Currencies>();
                });
        }


    }
}
