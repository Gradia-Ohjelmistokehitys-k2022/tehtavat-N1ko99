using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rajapinnat.Model
{
    public class DataPrice
    {
        public (decimal lowestPrice, DateTime lowestPriceDate, decimal highestPrice, DateTime highestPriceDate) GetPriceData(List<Data> data)
        {
            //haetaan pienin hinta
            var lowestPrice = data.OrderBy(d => d.Price).First();
            //haetaan suurin hinta
            var highestPrice = data.OrderByDescending(d => d.Price).First();

            return (lowestPrice.Price, lowestPrice.Date, highestPrice.Price, highestPrice.Date);
        }
    }

}
