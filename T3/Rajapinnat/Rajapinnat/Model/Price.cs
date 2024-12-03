using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Rajapinnat.Model;

namespace Rajapinnat.Model
{
    public class Price
    {

        private const string Api = "https://api.coingecko.com/api/v3/coins/bitcoin/market_chart/range";
        public List<Data> GetBitcoinData(long from, long to)
        {
            var client = new RestClient(Api);
            var request = new RestRequest();
            request.Method = Method.Get;
            request.AddParameter("vs_currency", "eur");
            request.AddParameter("from", from);
            request.AddParameter("to", to);

            var response = client.Execute(request);
            var data = JObject.Parse(response.Content);

            var prices = data["prices"].ToObject<List<List<object>>>();
            var volumes = data["total_volumes"].ToObject<List<List<object>>>();

            var bitcoinData = new List<Data>();

            for (int i = 0; i < prices.Count; i++)
            {
                bitcoinData.Add(new Data
                {
                    Date = DateTimeOffset.FromUnixTimeMilliseconds((long)prices[i][0]).DateTime,
                    Price = Convert.ToDecimal(prices[i][1]),
                    Volume = Convert.ToDecimal(volumes[i][1])
                });
            }

            return bitcoinData;
        }
    }
}






        
