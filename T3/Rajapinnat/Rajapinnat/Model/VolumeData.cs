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
            public void DisplayVolumeData(List<Data> data, TextBox textBox5, TextBox textBox6, TextBox textBox7, TextBox textBox8)
            {
                var lowestVolume = data.OrderBy(d => d.Volume).First();
                var highestVolume = data.OrderByDescending(d => d.Volume).First();

                textBox5.Text = $"{lowestVolume.Volume}";
                textBox7.Text = $"{lowestVolume.Date}";
                textBox6.Text = $"{highestVolume.Volume}";
                textBox8.Text = $"{highestVolume.Date}";
            }
        }
    }
