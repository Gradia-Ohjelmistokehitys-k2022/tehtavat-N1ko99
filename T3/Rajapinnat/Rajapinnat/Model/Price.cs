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
                // Luo RestClient-objekti API-osoitteella
                var client = new RestClient(Api);
                var request = new RestRequest
                {
                    Method = Method.Get
                };
                // Lisää parametrit pyyntöön
                request.AddParameter("vs_currency", "eur");
                request.AddParameter("from", from);
                request.AddParameter("to", to);

                // Suorita pyyntö ja tallenna vastaus
                var response = client.Execute(request);

                // tarkistetaan vastaus ja että se ei ole tyhjä
                if (response.StatusCode != System.Net.HttpStatusCode.OK || string.IsNullOrEmpty(response.Content))
                {
                    MessageBox.Show("Virhe API-yhteydessä tai tyhjä vastaus saatu.");
                    return bitcoinData;
                }

                var data = JObject.Parse(response.Content);  // Jäsennetään JSON-vastaus


                // Haetaan hinnat ja volyymit JSON-datasta
                var prices = data["prices"]?.ToObject<List<List<object>>>();
                var volumes = data["total_volumes"]?.ToObject<List<List<object>>>();

                //Tarkista hinta ja volyymi
                if (prices == null || volumes == null || prices.Count != volumes.Count)
                {
                    MessageBox.Show("Vastauksessa oli puutteellisia tai virheellisiä tietoja.");
                    return bitcoinData; // Palautetaan tyhjä lista
                }

                for (int i = 0; i < prices.Count; i++)
                {
                    bitcoinData.Add(new Data
                    {
                        // Muunnetaan Unix-aikamerkki DateTime-objektiksi ja asetetaan Date-kenttään
                        Date = DateTimeOffset.FromUnixTimeMilliseconds((long)prices[i][0]).DateTime,
                        // Muunnetaan hinta desimaaliluvuksi ja asetetaan Price-kenttään
                        Price = Convert.ToDecimal(prices[i][1]),
                        // Muunnetaan volyymi desimaaliluvuksi ja asetetaan Volume-kenttään
                        Volume = Convert.ToDecimal(volumes[i][1])
                    });
                }
            }
            catch (Exception ex)
            {
                // Käsitellään poikkeukset
                MessageBox.Show($"Virhe API-yhteydessä: {ex.Message}");
            }
            return bitcoinData; // Palautetaan lista
        }
    }
}
