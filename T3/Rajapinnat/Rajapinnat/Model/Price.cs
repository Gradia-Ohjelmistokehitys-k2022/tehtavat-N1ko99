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
using System.Windows.Forms;

namespace Rajapinnat.Model
{
    public class Price
    {

        private const string Api = "https://api.coingecko.com/api/v3/coins/bitcoin/market_chart/range";
        public List<Data> GetBitcoinData(long from, long to)
        {
            var bitcoinData = new List<Data>();

            try
            {
                var client = new RestClient(Api);
                var request = new RestRequest
                {
                    Method = Method.Get
                };
                request.AddParameter("vs_currency", "eur");
                request.AddParameter("from", from);
                request.AddParameter("to", to);

                var response = client.Execute(request);

                // Tarkista, että vastaus saadaan
                if (response.StatusCode != System.Net.HttpStatusCode.OK || string.IsNullOrEmpty(response.Content))
                {
                    MessageBox.Show("Virhe API-yhteydessä tai tyhjä vastaus saatu.");
                    return bitcoinData;
                }

                // Jäsennetään json
                var data = JObject.Parse(response.Content);

                // Haetaan hinnat ja volyymit JSON-datasta
                var prices = data["prices"]?.ToObject<List<List<object>>>();
                var volumes = data["total_volumes"]?.ToObject<List<List<object>>>();

                if (prices == null || volumes == null || prices.Count != volumes.Count)
                {
                    MessageBox.Show("Vastauksessa oli puutteellisia tai virheellisiä tietoja.");
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
                MessageBox.Show($"Virhe API-yhteydessä: {ex.Message}");
            }

            return bitcoinData; // Palautetaan joko täytetty tai tyhjä lista
        }
    }
}

