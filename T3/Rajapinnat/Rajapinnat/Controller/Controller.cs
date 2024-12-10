using System.Collections.Generic;
using System;
using System.Linq;

namespace Rajapinnat.Model
{
    public class BitcoinController
    {
        private Price _service;
        private VolumeData _volumeData;
        private Trends _trends;

        public BitcoinController()
        {
            _service = new Price();
            _volumeData = new VolumeData();
            _trends = new Trends();
        }

        public List<Data> GetBitcoinData(long from, long to)
        {
            return _service.GetBitcoinData(from, to);
        }

        public (int longestBearish, DateTime? bearishStart, DateTime? bearishEnd) GetBearishTrend(List<Data> data)
        {
            var bitcoinPrices = data.Select(d => Tuple.Create(d.Date, (double)d.Price)).ToList();
            return _trends.GetLenghtOfTrends(bitcoinPrices, false);
        }

        public (int longestBullish, DateTime? bullishStart, DateTime? bullishEnd) GetBullishTrend(List<Data> data)
        {
            var bitcoinPrices = data.Select(d => Tuple.Create(d.Date, (double)d.Price)).ToList();
            return _trends.GetLenghtOfTrends(bitcoinPrices, true);
        }


        public (DateTime bestBuy, DateTime bestSell) GetBestBuySellDays(List<Data> data)
        {
            var buySell = new BuySell();
            return (buySell.BestDayToBuy(data), buySell.BestDayToSell(data));
        }


        public (decimal lowestVolume, DateTime lowestVolumeDate, decimal highestVolume, DateTime highestVolumeDate) GetVolumeData(List<Data> data)
        {
            return _volumeData.GetVolumeData(data);
        }
    }
}
