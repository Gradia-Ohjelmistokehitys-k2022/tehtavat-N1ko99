﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rajapinnat.Model
{
    public class Trends
    {
        public int BearishTrend(List<Data> data)
        {
            int longestBearish = 0;
            int currentBearish = 0;

            for (int i = 1; i < data.Count; i++)
            {
                if (data[i].Price < data[i - 1].Price)
                {
                    currentBearish++;
                    if (currentBearish > longestBearish)
                    {
                        longestBearish = currentBearish;
                    }
                }
                else
                {
                    currentBearish = 0;
                }
            }

            // Jos longestBearish on suurempi kuin 0, lisätään 1, koska laskeminen alkaa ensimmäisestä päivästä
            return longestBearish > 0 ? longestBearish + 1 : longestBearish;
        }


        public int BullishTrend(List<Data> data)
        {
            int longestBullish = 0;
            int currentBullish = 0;

            for (int i = 1; i < data.Count; i++)
            {
                if (data[i].Price > data[i - 1].Price)
                {
                    currentBullish++;
                    if (currentBullish > longestBullish)
                    {
                        longestBullish = currentBullish;
                    }
                }
                else
                {
                    currentBullish = 0;
                }
            }

            // Jos longestBullish on suurempi kuin 0, lisätään 1, koska laskeminen alkaa ensimmäisestä päivästä
            return longestBullish > 0 ? longestBullish + 1 : longestBullish;
        }

    }
}
