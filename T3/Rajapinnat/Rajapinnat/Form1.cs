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
            //tarkistetaan ettei aloituspäivämäärä ole jälkeen lopetuspäivämäärän
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
            DisplayPriceData(data);
            DisplayVolumeData(data);

            // Näytä trendit
            (int longestBearish, DateTime? bearishStart, DateTime? bearishEnd) = _controller.GetBearishTrend(data);
            (int longestBullish, DateTime? bullishStart, DateTime? bullishEnd) = _controller.GetBullishTrend(data);
            DisplayTrends(longestBearish, bearishStart, bearishEnd, longestBullish, bullishStart, bullishEnd);

            // Näytä paras ostoaika ja myyntiaika
            var bestBuySellDays = _controller.GetBestBuySellDays(data);
            DisplayBestBuySellDays(bestBuySellDays);
            // Piirrä kaavio
            DrawChart(data);
        }

        //asetetaan textboxeihin matalin ja korkein hinta
        private void DisplayPriceData(List<Data> data)
        {
            var lowestPrice = data.OrderBy(d => d.Price).First();
            var highestPrice = data.OrderByDescending(d => d.Price).First();

            textBox1.Text = $"{lowestPrice.Price}";
            textBox2.Text = $"{highestPrice.Price}";
            textBox3.Text =  $"{lowestPrice.Date:dd-MM-yyyy}";
            textBox4.Text = $"{highestPrice.Date:dd-MM-yyyy}";
        }
        //Asetetaan voluumit textboxeihin
        private void DisplayVolumeData(List<Data> data)
        {
            var (lowestVolume, lowestVolumeDate, highestVolume, highestVolumeDate) = _controller.GetVolumeData(data);

            textBox5.Text = $"{lowestVolume}";
            textBox7.Text = $"{lowestVolumeDate:dd-MM-yyyy}";
            textBox6.Text = $"{highestVolume}";
            textBox8.Text = $"{highestVolumeDate:dd-MM-yyyy}";
        }
        //asetetaan trendit bearish ja bullish textboxeihin
        private void DisplayTrends(int longestBearish, DateTime? bearishStart, DateTime? bearishEnd, int longestBullish, DateTime? bullishStart, DateTime? bullishEnd)
        {
            textBox9.Text = $"{bearishStart:dd-MM-yyyy}";
            textBox15.Text = $"{bearishEnd:dd-MM-yyyy}";

            textBox11.Text = $"{bullishStart:dd-MM-yyyy}";
            textBox13.Text = $"{bullishEnd:dd-MM-yyyy}";
        }
        //asetetaan paraspäivä ostaa ja myydä tetboxeihin
        private void DisplayBestBuySellDays((DateTime bestBuy, DateTime bestSell) bestDays)
        {
            textBox14.Text = $"{bestDays.bestBuy:dd-MM-yyyy}";
            textBox10.Text = $"{bestDays.bestBuy:hh:mm}";

            textBox16.Text = $"{bestDays.bestSell:dd-MM-yyyy}";
            textBox12.Text = $"{bestDays.bestSell:hh:mm}";
        }

        private void DrawChart(List<Data> data)
        {
            //tyhjennetään kaavio alueet
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            //määritetään minimi ja maksimi arvot hinnalle
            decimal minValue = data.Min(d => d.Price);
            decimal maxValue = data.Max(d => d.Price);
            //luodaan kaavio ja asetetaan asetukset siihen
            var chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea
            {
                Name = "MainArea",
                AxisY = { LabelStyle = { Format = "C" }, Minimum = (double)(minValue * 0.95m), Maximum = (double)(maxValue * 1.05m) },
                AxisX = { MajorGrid = { Enabled = false } }
            };
            
            chart1.ChartAreas.Add(chartArea);
            //luodaan sarja
            var series = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Bitcoinin hinta kaavio",
                Color = System.Drawing.Color.Green,
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
            };

            chart1.Series.Add(series);
            //lisätään datapisteet sarjaan
            foreach (var item in data)
            {
                series.Points.AddXY(item.Date, item.Price);
            }

            series.BorderWidth = 1;
            //piilotetaan otsikko
            if (chart1.Titles.Count > 0)
            {
                chart1.Titles[0].Visible = false;
            }
            //päivitetään
            chart1.Invalidate();
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            var hit = chart1.HitTest(e.X, e.Y);
           // Tarkistetaan onko hiiri datapisteen päällä
            if (hit.ChartElementType == System.Windows.Forms.DataVisualization.Charting.ChartElementType.DataPoint)
            {
                var pointIndex = hit.PointIndex;
                var point = hit.Series.Points[pointIndex];
                var date = DateTime.FromOADate(point.XValue).ToString("dd-MM-yyyy HH:mm:ss");
                var price = point.YValues[0].ToString("C");
                //näytetään tooltipissä päivämäärä sekä kello ja hinta
                toolTip1.Show($"Päivä: {date} Hinta: {price}", chart1, e.Location.X + 10, e.Location.Y + 10);
            }
            else
            {
               // Piilotetaan tooltip kun hiiri ei ole sen päällä
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
