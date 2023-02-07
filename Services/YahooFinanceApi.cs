using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StockDataGenerator.Services
{
    public class YahooFinanceApi : IYahooFinanceApi
    {
        private readonly ILogger<YahooFinanceApi> _logger;

        public YahooFinanceApi(ILogger<YahooFinanceApi> logger)
        {
            this._logger = logger;
        }


        /// <summary>
        /// Obtiene las cotizaciones de las acciones indicadas
        /// </summary>
        /// <returns></returns>
        public List<Business.Model.YahooFinanceQuotes> GetQuotes(List<Repositories.Model.Entities.Quotes> quotesList)
        {
            List<Business.Model.YahooFinanceQuotes> list = new List<Business.Model.YahooFinanceQuotes>();
            //lista de stocks separada por ","
            List<string> stocksToUrl = quotesList.Select(q => q.symbol).ToList();
            var url = $"https://query1.finance.yahoo.com/v7/finance/quote?symbols=" + string.Join(",", stocksToUrl);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) throw new WebException($"ERROR EN {url}");
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            // Do something with responseBody
                            Console.WriteLine(responseBody);
                            //Creamos la lista de quotes
                            JObject jResponse = JObject.Parse(responseBody);

                            JArray quotes = (JArray)jResponse["quoteResponse"]["result"];
                            list = JsonConvert.DeserializeObject<List<Business.Model.YahooFinanceQuotes>>(quotes.ToString());

                        }
                    }
                }

                return list;


            }
            catch (WebException ex)
            {
                // Handle error
                throw new WebException(ex.Message);
            }




        }




    }
}
