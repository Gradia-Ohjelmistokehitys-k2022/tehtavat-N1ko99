using Rajapinnat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.Diagnostics;

namespace Rajapinnat
{
    public partial class Form1 : Form
    {
        private BitcoinController _controller;



        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Otetaan arvot DateTimePickeristä ja asetetaan ne unix aikaan
            long from = new DateTimeOffset(dateTimePicker1.Value).ToUnixTimeSeconds();
            long to = new DateTimeOffset(dateTimePicker2.Value).ToUnixTimeSeconds();
            //Haetaan tiedot Bitcoinista getBitcoinData metodin avulla käyttäen arvoja from ja to
            BitcoinController bitcoinController = new BitcoinController();
            var data = bitcoinController.getBitcoinData(from, to);
            //tarkistetaan ettei arvo ole null
            if (data == null || !data.Any())
            {
                MessageBox.Show("Ei tietoja valitulta aikaväliltä.");
                return;
            }
            //asetetaan arvot alhaisin hinta, korkein hinta, pienin voluumi sekä korkein voluumi muuttujiin
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

            //Trendit
            //Pisin BearishTrendi
            var trend = new Trends();
            int longestBearish = trend.BearishTrend(data);
            textBox9.Text = $"{longestBearish}";
            
            //Pisin BullishTrendi
            int longestBullish = trend.BullishTrend(data);
            textBox11.Text = $"{longestBullish}";

            //Milloin kannattaa ostaa
            var buysell = new BuySell();
            DateTime bestDayToBuy = buysell.BestDayToBuy(data);
            textBox14.Text = $"{bestDayToBuy:dd-MM-yyyy-hh-mm}";

            //Milloin myydä
            DateTime bestDayToSell = buysell.BestDayToSell(data);
            textBox16.Text = $"{bestDayToSell:dd-MM-yyyy-hh-mm}";
        }

        // tehdään kaavio bitcoinin hinnasta
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

        //kun siirretään hiiri kaavion kohtaan näytetään tooltipissä Hinta ja Päivämäärä
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
