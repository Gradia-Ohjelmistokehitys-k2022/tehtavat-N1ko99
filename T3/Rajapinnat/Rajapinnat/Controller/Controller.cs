using Newtonsoft.Json.Linq;
using Rajapinnat.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Rajapinnat.Model
{
    public class BitcoinController
    {
        private Price _service;
        public BitcoinController()
        {
            _service = new Price();
        }

            Price price = new Price();
        public List<Data> getBitcoinData(long from, long to)
        {
            return price.GetBitcoinData(from, to);
        }

        public List<Data> GetBitcoinData(long from, long to)
        {
            return _service.GetBitcoinData(from, to);
        }

        public (int longestBearish, int longestBullish) GetTrends(List<Data> data)
        {
            var trends = new Trends();
            int longestBearish = trends.BearishTrend(data);
            int longestBullish = trends.BullishTrend(data);
            return (longestBearish, longestBullish);
        }


        public (DateTime bestBuy, DateTime bestSell) GetBestBuySellDays(List<Data> data)
        {
            var buySell = new BuySell();
            return (buySell.BestDayToBuy(data), buySell.BestDayToSell(data));
        }

        public (Data lowestPrice, Data highestPrice, Data lowestVolume, Data highestVolume) GetPriceVolumeData(List<Data> data)
        {
            var priceData = new DataPrice();
            var volumeData = new VolumeData();

            var lowestPrice = data.OrderBy(d => d.Price).First();
            var highestPrice = data.OrderByDescending(d => d.Price).First();
            var lowestVolume = data.OrderBy(d => d.Volume).First();
            var highestVolume = data.OrderByDescending(d => d.Volume).First();

            return (lowestPrice, highestPrice, lowestVolume, highestVolume);
        }
    }
}


