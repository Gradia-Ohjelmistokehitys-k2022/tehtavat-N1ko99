using Rajapinnat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Rajapinnat.Model;

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

            if (from >= to)
            {
                MessageBox.Show("Aloituspäivämäärän on oltava ennen lopetuspäivämäärää.");
                return;
            }

            var data = _controller.GetBitcoinData(from, to);

            if (data == null || !data.Any())
            {
                MessageBox.Show("Ei tietoja valitulta aikaväliltä.");
                return;
            }

            DisplayData(data);
        }

        private void DisplayData(List<Data> data)
        {
            // Näytä hinnat ja volyymit
            var priceVolumeData = _controller.GetPriceVolumeData(data);
            DisplayPriceVolumeData(priceVolumeData);

            // Näytä trendit
            var trends = _controller.GetTrends(data);
            DisplayTrends(trends);

            // Näytä paras ostoaika ja myyntiaika
            var bestBuySellDays = _controller.GetBestBuySellDays(data);
            DisplayBestBuySellDays(bestBuySellDays);

            // Piirrä kaavio
            DrawChart(data);
        }

        private void DisplayPriceVolumeData((Data lowestPrice, Data highestPrice, Data lowestVolume, Data highestVolume) data)
        {
            textBox1.Text = $"{data.lowestPrice.Price}€";
            textBox2.Text = $"{data.highestPrice.Price}€";
            textBox3.Text = $"{data.highestPrice.Date}";
            textBox4.Text = $"{data.lowestPrice.Date}";

            textBox5.Text = $"{data.lowestVolume.Volume}";
            textBox7.Text = $"{data.lowestVolume.Date}";
            textBox6.Text = $"{data.highestVolume.Volume}";
            textBox8.Text = $"{data.highestVolume.Date}";
        }

        private void DisplayTrends((int longestBearish, int longestBullish) trends)
        {
            textBox9.Text = $"{trends.longestBearish}";
            textBox11.Text = $"{trends.longestBullish}";
        }

        private void DisplayBestBuySellDays((DateTime bestBuy, DateTime bestSell) bestDays)
        {
            textBox14.Text = $"{bestDays.bestBuy:dd-MM-yyyy}";
            textBox10.Text = $"{bestDays.bestBuy:hh:mm}";

            textBox16.Text = $"{bestDays.bestSell:dd-MM-yyyy}";
            textBox12.Text = $"{bestDays.bestSell:hh:mm}";
        }

        private void DrawChart(List<Data> data)
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            decimal minValue = data.Min(d => d.Price);
            decimal maxValue = data.Max(d => d.Price);

            var chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea
            {
                Name = "MainArea",
                AxisY = { LabelStyle = { Format = "C" }, Minimum = (double)(minValue * 0.95m), Maximum = (double)(maxValue * 1.05m) },
                AxisX = { MajorGrid = { Enabled = false } }
            };

            chart1.ChartAreas.Add(chartArea);

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

            series.BorderWidth = 1;
            if (chart1.Titles.Count > 0)
            {
                chart1.Titles[0].Visible = false;
            }
            chart1.Invalidate();
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            var hit = chart1.HitTest(e.X, e.Y);

            if (hit.ChartElementType == System.Windows.Forms.DataVisualization.Charting.ChartElementType.DataPoint)
            {
                var pointIndex = hit.PointIndex;
                var point = hit.Series.Points[pointIndex];
                var date = DateTime.FromOADate(point.XValue).ToString("dd-MM-yyyy HH:mm:ss");
                var price = point.YValues[0].ToString("C");

                toolTip1.Show($"Päivä: {date} Hinta: {price}", chart1, e.Location.X + 10, e.Location.Y + 10);
            }
            else
            {
                toolTip1.Hide(chart1);
            }
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}