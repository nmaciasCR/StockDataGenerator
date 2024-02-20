﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StockDataGenerator.Business.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StockDataGenerator.Services
{
    public class YahooFinanceApi : IYahooFinanceApi
    {
        Business.Interfaces.IWriter _writer;
        String URL_STOCKS;
        String API_KEY;


        public YahooFinanceApi(IConfiguration configuration, Business.Interfaces.IWriter writer)
        {
            _writer = writer;
            URL_STOCKS = configuration["Services:YahooFinance:url"];
            API_KEY = configuration["Services:YahooFinance:apiKey"];
        }


        /// <summary>
        /// Obtiene las cotizaciones de las acciones indicadas
        /// </summary>
        /// <returns></returns>
        public async Task<List<YahooFinanceQuotes>> GetQuotesAsync(List<Repositories.Model.Entities.Quotes> quotesList)
        {
            List<Business.Model.YahooFinanceQuotes> list = new List<YahooFinanceQuotes>();
            //lista de stocks separada por ","
            List<string> stocksToUrl = quotesList.Select(q => q.symbol).ToList();
            var url = URL_STOCKS + string.Join(",", stocksToUrl);

            try
            {

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-API-KEY", API_KEY);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var clientRequest = new HttpRequestMessage(HttpMethod.Get, url);
                    clientRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.SendAsync(clientRequest).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string stocksResponse = await response.Content.ReadAsStringAsync();
                        //Creamos la lista de quotes
                        JObject jResponse = JObject.Parse(stocksResponse);
                        JArray quotes = (JArray)jResponse["quoteResponse"]["result"];
                        list = JsonConvert.DeserializeObject<List<YahooFinanceQuotes>>(quotes.ToString());
                    }
                    else
                    {
                        _writer.writeLine.Error($"ERROR EN METODO client.SendAsync({url})");
                    }
                }

                return list;
            }
            catch (WebException ex)
            {
                _writer.writeLine.Error("ERROR: YahooFinance - GetQuotes() - WebException: " + ex.Message);
                // Handle error
                return list;
            }




        }




    }
}
