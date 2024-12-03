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
        private DataPrice _priceData;
        private VolumeData _volumeData;
        private Trends _trends;
        private BuySell _buySell;
        public Form1()
        {
            InitializeComponent();
            _controller = new BitcoinController();
            _priceData = new DataPrice();
            _volumeData = new VolumeData();
            _trends = new Trends();
            _buySell = new BuySell();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            long from = new DateTimeOffset(dateTimePicker1.Value).ToUnixTimeSeconds();
            long to = new DateTimeOffset(dateTimePicker2.Value).ToUnixTimeSeconds();
            BitcoinController bitcoinController = new BitcoinController();
            var data = bitcoinController.getBitcoinData(from, to);
            // Tarkistaa, että aloituspäivämäärä on ennen lopetuspäivämäärää
            if (from >= to)
            {
                MessageBox.Show("Aloituspäivämäärän on oltava ennen lopetuspäivämäärää.");
                return;
            }
            // Tarkistaa, että dataa on saatavilla valitulta aikaväliltä
            if (data == null || !data.Any())
            {
                MessageBox.Show("Ei tietoja valitulta aikaväliltä.");
                return;
            }
            // Suodattaa tiedot haetun aikavälin mukaan
            var filteredData = data.Where(d => d.Date >= dateTimePicker1.Value && d.Date <= dateTimePicker2.Value).ToList();
            //Näytä Hinta suurin ja pienin
            _priceData.DisplayPriceData(filteredData, textBox1, textBox2, textBox3, textBox4);
            //Näytä volyymi tiedot suurin ja pienin
            _volumeData.DisplayVolumeData(filteredData, textBox5, textBox6, textBox7, textBox8);
            //näytä trendit bearish bullish
            DisplayTrends(filteredData);
            //näytä paras aika ostaa ja myydä
            DisplayBestBuySellDays(filteredData);
            //piirrä kaavio
            DrawChart(filteredData);
        }

        private void DisplayTrends(List<Data> data)
        {
            //Lasketaan pisin nousu ja lasku päivinä
            int longestBearish = _trends.BearishTrend(data);
            int longestBullish = _trends.BullishTrend(data);

            textBox9.Text = $"{longestBearish}";
            textBox11.Text = $"{longestBullish}";
        }

        private void DisplayBestBuySellDays(List<Data> data)
        {
            //lasketaan paras aika ostaa ja myyd'
            DateTime bestDayToBuy = _buySell.BestDayToBuy(data);
            DateTime bestDayToSell = _buySell.BestDayToSell(data);

            textBox14.Text = $"{bestDayToBuy:dd-MM-yyyy}";
            textBox10.Text = $"{bestDayToBuy:hh:mm}";

            textBox16.Text = $"{bestDayToSell:dd-MM-yyyy}";
            textBox12.Text = $"{bestDayToSell:hh:mm}";


        }

        private void DrawChart(List<Data> data)
        {
            //piirretään chartti
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            decimal minValue = data.Min(d => d.Price);
            decimal maxValue = data.Max(d => d.Price);

            var chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea
            {
                Name = "MainArea",
                AxisY = { LabelStyle = { Format = "C" }, Minimum = (double)(minValue * 0.95m), Maximum = (double)(maxValue * 1.05m) },
                AxisX = { MajorGrid = { Enabled = false }}
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
            if (chart1.Titles.Count > 0)
            {
                chart1.Titles[0].Visible = false;
            }
            chart1.Invalidate();
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {//kun liikutetaan hiirtä chartila näytetään hinta ja päivämäärä sekä aika
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
    }
}
