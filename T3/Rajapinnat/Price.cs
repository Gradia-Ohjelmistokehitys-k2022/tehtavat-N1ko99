using System;

public class Price
{

     public async Task FetchBitcoinDataAsync(DateTime startDate, DateTime endDate)
    {
        long fromUnix = DateTimeToUnixTimestamp(startDate);
        long toUnix = DateTimeToUnixTimestamp(endDate.AddHours(1));
        string url = $"https://api.coingecko.com/api/v3/coins/bitcoin/market_chart/range?vs_currency=eur&from={fromUnix}&to={toUnix}";
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetStringAsync(url);
            var marketData = JsonConvert.DeserializeObject<MarketData>(response);
            ProcessMarketData(marketData);
        }
    }


}

