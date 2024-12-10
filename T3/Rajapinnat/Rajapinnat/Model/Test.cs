using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rajapinnat.Model
{
    public class Test
    {
    
        private const string Api = "https://api.coingecko.com/api/v3/coins/bitcoin/market_chart/range";
        private readonly HttpClient _httpClient;

        public Test()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Data>> GetBitcoinDataAsync(long from, long to)
        {
            var bitcoinData = new List<Data>();

            try
            {
                var url = $"{Api}?vs_currency=eur&from={from}&to={to}";
                var response = await _httpClient.GetAsync(url);

                // Tarkista, että vastaus saadaan
                if (!response.IsSuccessStatusCode || string.IsNullOrEmpty(response.Content.ReadAsStringAsync().Result))
                {
                    Console.WriteLine("Virhe API-yhteydessä tai tyhjä vastaus saatu.");
                    return bitcoinData;
                }

                // Jäsennetään json
                var data = JObject.Parse(await response.Content.ReadAsStringAsync());

                // Haetaan hinnat ja volyymit JSON-datasta
                var prices = data["prices"]?.ToObject<List<List<object>>>();
                var volumes = data["total_volumes"]?.ToObject<List<List<object>>>();

                if (prices == null || volumes == null || prices.Count != volumes.Count)
                {
                    Console.WriteLine("Vastauksessa oli puutteellisia tai virheellisiä tietoja.");
                    return bitcoinData; // Palautetaan tyhjä lista
                }

                // Rakennetaan Data-olioita tuloslistaan
                for (int i = 0; i < prices.Count; i++)
                {
                    bitcoinData.Add(new Data
                    {
                        Date = DateTimeOffset.FromUnixTimeMilliseconds((long)prices[i][0]).DateTime,
                        Price = Convert.ToDecimal(prices[i][1]),
                        Volume = Convert.ToDecimal(volumes[i][1])
                    });
                }
            }
            catch (Exception ex)
            {
                // Kaikki verkko- tai yleiset poikkeukset
                Console.WriteLine($"Virhe API-yhteydessä: {ex.Message}");
            }

            return bitcoinData; // Palautetaan joko täytetty tai tyhjä lista
        }
    }

}
