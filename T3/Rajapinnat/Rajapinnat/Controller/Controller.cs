using Newtonsoft.Json.Linq;
using Rajapinnat.Model;
using RestSharp;
using System;
using System.Collections.Generic;


namespace Rajapinnat.Model
{
    public class BitcoinController
    {
        Price price = new Price();
        public List<Data> getBitcoinData(long from, long to)
        {
            return price.GetBitcoinData(from, to);
        }
    }
}