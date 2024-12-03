using Rajapinnat.Controller;
using Rajapinnat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

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

            //min hinta
            textBox1.Text = $"{lowestPrice.Price}€";
            textBox2.Text = $"{highestPrice.Price}€";
            //max hinta
            textBox3.Text = $"{highestPrice.Date}";
            textBox4.Text = $"{lowestPrice.Date}";

            DrawChart(data);
            //volyymi min
             textBox5.Text = $"{lowestVolume.Volume}";
            textBox7.Text = $"{lowestVolume.Date}";
            //volyymi max
            textBox6.Text = $"{highestVolume.Volume}";
            textBox8.Text = $"{highestVolume.Date}";

            var trend = new Trends();
            int longestBearish = trend.BearishTrend(data);

            textBox9.Text = $"{longestBearish}";

            int longestBullish = trend.BullishTrend(data);
            textBox11.Text = $"{longestBullish}";


            var buysell = new BuySell();
            DateTime bestDayToBuy = buysell.BestDayToBuy(data);
            textBox14.Text = $"{bestDayToBuy:dd-MM-yyyy-hh-mm}";

            DateTime bestDayToSell = buysell.BestDayToSell(data);
            textBox16.Text = $"{bestDayToSell:dd-MM-yyyy-hh-mm}";
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
                AxisY = { LabelStyle = { Format = "C" }, Minimum = (double)(minValue * 0.95m), Maximum = (double)(maxValue * 1.05m) }
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

            series.BorderWidth = 2;
            chart1.Invalidate();
        }


        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            var hit = chart1.HitTest(e.X, e.Y);

            if (hit.ChartElementType == System.Windows.Forms.DataVisualization.Charting.ChartElementType.DataPoint)
            {
                var pointIndex = hit.PointIndex;
                var point = hit.Series.Points[pointIndex];
                var date = DateTime.FromOADate(point.XValue).ToShortDateString();
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
    }
}
