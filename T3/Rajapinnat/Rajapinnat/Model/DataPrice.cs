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
            public void DisplayPriceData(List<Data> data, TextBox textBox1, TextBox textBox2, TextBox textBox3, TextBox textBox4)
            {
                var lowestPrice = data.OrderBy(d => d.Price).First();
                var highestPrice = data.OrderByDescending(d => d.Price).First();

                textBox1.Text = $"{lowestPrice.Price}€";
                textBox2.Text = $"{highestPrice.Price}€";
                textBox3.Text = $"{highestPrice.Date}";
                textBox4.Text = $"{lowestPrice.Date}";
            }
        }
    }
