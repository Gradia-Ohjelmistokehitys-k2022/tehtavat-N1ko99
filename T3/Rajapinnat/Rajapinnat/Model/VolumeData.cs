using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Rajapinnat.Model
{
    public class VolumeData
    {
            public (decimal lowestVolume, DateTime lowestVolumeDate, decimal highestVolume, DateTime highestVolumeDate) GetVolumeData(List<Data> data)
            {
            // Järjestetään data volyymin perusteella nousevaan järjestykseen ja haetaan pienin volyymi                var lowestVolume = data.OrderBy(d => d.Volume).First();
                var lowestVolume = data.OrderBy(d => d.Volume).First();
            // Järjestetään data volyymin perusteella laskevaan järjestykseen ja haetaan suurin volyymi
                var highestVolume = data.OrderByDescending(d => d.Volume).First();
                //palautetaan
                return (lowestVolume.Volume, lowestVolume.Date, highestVolume.Volume, highestVolume.Date);
            }
        }
    }

