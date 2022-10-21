using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XML_SOAP_gyakorlat.MnbServiceReference;
using XML_SOAP_gyakorlat.Entities;
using System.Xml;
using System.Windows.Forms.DataVisualization.Charting;

namespace XML_SOAP_gyakorlat
{
    public partial class Form1 : Form
    {
        BindingList<RateData> Rates = new BindingList<RateData>();

        public Form1()
        {
            InitializeComponent();

            dataGridView1.DataSource = Rates;
            chartRateData.DataSource = Rates;

            Start();
            XML_process(Start());
            Display();
        }

        private string Start()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();

            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = "EUR",
                startDate = "2020-01-01",
                endDate = "2020-06-30"
            };

            var response = mnbService.GetExchangeRates(request);

            var result = response.GetExchangeRatesResult;

            return result;
        }


        private void XML_process(string result)
        {
            XmlDocument xml = new XmlDocument();

            xml.LoadXml(result);

            foreach (XmlElement element in xml.DocumentElement)
            {
                var rate = new RateData();

                

                rate.Date = DateTime.Parse(element.GetAttribute("date"));

                var childElement = (XmlElement)element.ChildNodes[0];
                rate.Currency = childElement.GetAttribute("curr");

                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);

                if (unit != 0)
                {
                    rate.Value = value / unit;
                }

                Rates.Add(rate);
            }
        }

        private void Display()
        {
            var series = chartRateData.Series[0];

            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;

            var legend = chartRateData.Legends[0];
            legend.Enabled = false;

            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;

            chartArea.AxisY.IsStartedFromZero = false;
        }
    }
}
