using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rajapinnat.Model
{
    public class Trends
    {
        //public (int days, DateTime? start) BearishTrend(List<Data> data)
        //{
        //    int longestBearish = 0;
        //    int currentBearish = 0;
        //    DateTime? currentStart = null;
        //    DateTime? longestStart = null;

        //    for (int i = 1; i < data.Count; i++)
        //    {
        //        if (data[i].Price < data[i - 1].Price)
        //        {
        //            currentBearish++;
        //            if (currentBearish == 1)
        //            {
        //                currentStart = data[i - 1].Date;
        //            }

        //            if (currentBearish > longestBearish)
        //            {
        //                longestBearish = currentBearish;
        //                longestStart = currentStart;
        //            }
        //        }
        //        else
        //        {
        //            currentBearish = 0;
        //            currentStart = null;
        //        }
        //    }

        //    return longestBearish > 0 ? (longestBearish + 1, longestStart) : (0, null);
        //}



        //public int BullishTrend(List<Data> data)
        //{
        //    int longestBullish = 0;
        //    int currentBullish = 1; 

        //    for (int i = 1; i < data.Count; i++)
        //    {
        //        if (data[i].Price > data[i - 1].Price)
        //        {
        //            currentBullish++; // Jatka nousua
        //            longestBullish = Math.Max(longestBullish, currentBullish);
        //        }
        //        else
        //        {
        //            currentBullish = 1; // Nollataan
        //        }
        //    }

        //    return longestBullish;
        //}


        public (int length, DateTime? start, DateTime? end) GetLenghtOfTrends(List<Tuple<DateTime, double>> bitcoinPrices, bool isBullish)
        {
            // Tarkistetaan ettei lista ole tyhjä ja että siinä on vähintään kaksi elementtiä
            if (bitcoinPrices == null || bitcoinPrices.Count < 2) return (0, null, null);

            int longestTrend = 0;
            int currentTrend = 0;
            DateTime? currentStart = null;
            DateTime? longestStart = null;
            DateTime? longestEnd = null;

            // Käydään läpi hinnat
            for (int i = 1; i < bitcoinPrices.Count; i++)
            {
                // Tarkistetaan jatkuuko trendi
                bool isTrendContinuing = (isBullish && bitcoinPrices[i].Item2 > bitcoinPrices[i - 1].Item2) ||
                                         (!isBullish && bitcoinPrices[i].Item2 < bitcoinPrices[i - 1].Item2);

                if (isTrendContinuing)
                {
                    currentTrend++; // Kasvatetaan nykyisen trendin pituutta
                    if (currentStart == null)
                    {
                        currentStart = bitcoinPrices[i - 1].Item1; // Asetetaan nykyisen trendin aloituspäivämäärä
                    }
                }
                else
                {
                    // Jos nykyinen trendi on pisin päivitetään pisimmän trendin tiedot
                    if (currentTrend > longestTrend)
                    {
                        longestTrend = currentTrend;
                        longestStart = currentStart;
                        longestEnd = bitcoinPrices[i - 1].Item1;
                    }
                    currentTrend = 0;
                    currentStart = null;
                }
            }

            // Tarkistetaan onko nykyinen trendi pisin trendi
            if (currentTrend > longestTrend)
            {
                longestTrend = currentTrend;
                longestStart = currentStart;
                longestEnd = bitcoinPrices[bitcoinPrices.Count - 1].Item1;
            }

            return (longestTrend, longestStart, longestEnd); // Palautetaan pisimmän trendin pituus ja päivämäärät
        }
    }
}