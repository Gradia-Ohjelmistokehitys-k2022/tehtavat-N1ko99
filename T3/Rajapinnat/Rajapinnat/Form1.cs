using Rajapinnat.Controller;
using Rajapinnat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.DataVisualization.Charting;

namespace Rajapinnat
{
    public partial class Form1 : Form
    {
        private BitcoinController _controller;

        public Form1()
        {
            InitializeComponent();
            _controller = new BitcoinController();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            long from = new DateTimeOffset(dateTimePicker1.Value).ToUnixTimeSeconds();
            long to = new DateTimeOffset(dateTimePicker2.Value).ToUnixTimeSeconds();

            var data = _controller.GetBitcoinData(from, to);

            if (data == null || !data.Any())
            {
                MessageBox.Show("Ei tietoja valitulta aikaväliltä.");
                return;
            }

            var lowestPrice = data.OrderBy(d => d.Price).First();
            var highestPrice = data.OrderByDescending(d => d.Price).First();
            var lowestVolume = data.OrderBy(d => d.Volume).First();
            var highestVolume = data.OrderByDescending(d => d.Volume).First();

            textBox1.Text = $"{lowestPrice.Price}€";
            textBox2.Text = $"{highestPrice.Price}€";
            textBox3.Text = $"{highestPrice.Date}";
            textBox4.Text = $"{lowestPrice.Date}";

            DrawChart(data);
            // lblLowestVolume.Text = $"Lowest Volume: {lowestVolume.Volume} on {lowestVolume.Date}";
            // lblHighestVolume.Text = $"Highest Volume: {highestVolume.Volume} on {highestVolume.Date}";
        }

        private void DrawChart(List<Data> data)
        {
            chart1.Series.Clear();
            var series = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Bitcoinin hinta kaavio",
                Color = System.Drawing.Color.Green,
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
            };

            chart1.Series.Add(series);

            foreach (var item in data)
            {
                series.Points.AddXY(item.Date, item.Price);
            }

            chart1.Invalidate();
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
