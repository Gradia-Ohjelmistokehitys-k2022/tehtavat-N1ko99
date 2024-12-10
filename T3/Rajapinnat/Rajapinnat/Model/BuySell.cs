using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rajapinnat.Model
{
        public class BuySell
        {
            public DateTime BestDayToBuy(List<Data> data)
            {
                return data.OrderBy(pv => pv.Price).First().Date;
            }
            
            public DateTime BestDayToSell(List<Data> data)
            {
                return data.OrderByDescending(pv => pv.Price).First().Date;
            }
        }

    }
