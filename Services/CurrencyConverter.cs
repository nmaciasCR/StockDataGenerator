using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace StockDataGenerator.Services
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private Business.Interfaces.IWriter _writer;
        private string access_Key { get; set; }
        private string url { get; set; }

        public CurrencyConverter(IConfiguration configuration, Business.Interfaces.IWriter writer)
        {
            access_Key = configuration["Services:CurrencyConverter:access_key"];
            url = configuration["Services:CurrencyConverter:urlService"];
            _writer = writer;
        }


        /// <summary>
        /// Retorna las cotizaciones de las monedas solicitadas indicadas en euros
        /// </summary>
        public Business.Model.CurrencyConverterResponse GetCurrencyExchange(List<string> currencies)
        {

            Business.Model.CurrencyConverterResponse serviceResult = new Business.Model.CurrencyConverterResponse();

            try
            {
                string urlFull = url + "?access_key=" + access_Key + "&symbols=" + string.Join(",", currencies) + "&base=EUR";

                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(urlFull).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string exchanges = response.Content.ReadAsStringAsync().Result;
                        serviceResult = JsonConvert.DeserializeObject<Business.Model.CurrencyConverterResponse>(exchanges);
                        return serviceResult;
                    }
                    else
                    {
                        _writer.writeLine.Error($"ERROR EN METODO client.GetAsync({urlFull})");
                        return serviceResult;
                    }                    
                    //Console.WriteLine(result.Result);
                }

            }
            catch (Exception ex)
            {
                _writer.writeLine.Error($"ERROR EN METODO GetCurrencyExchange({string.Join(",", currencies)})");
                return serviceResult;
            }
        }




    }
}
