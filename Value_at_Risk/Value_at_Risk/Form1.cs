using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Value_at_Risk.Entities;

namespace Value_at_Risk
{
    public partial class Form1 : Form
    {
        List<Tick> Ticks;
        List<PortfolioItem> Portfolio = new List<PortfolioItem>();
        PortfolioEntities context = new PortfolioEntities();


        public Form1()
        {
            InitializeComponent();

            Ticks = context.Tick.ToList();
            dataGridView1.DataSource = Ticks;

            CreatePortfolio();
            //GetPortfolioValue();


            int elemszám = Portfolio.Count();
            decimal részvényekSzáma = (from x in Portfolio
                                       select x.Volume).Sum();

            MessageBox.Show(string.Format("Részvények száma: {0}", részvényekSzáma));

            DateTime minDatum = (from x in Ticks
                                 select x.TradingDay).Min();

            DateTime maxDatum = (from x in Ticks
                                 select x.TradingDay).Max();

            int elteltNapoSzama = (maxDatum - minDatum).Days;

            DateTime otpMinDatum = (from x in Ticks
                                    where x.Index == "OTP"
                                    select x.TradingDay).Min();

            var kapcsolt = from
                           x in Ticks
                           join
                          y in Portfolio
                           on x.Index equals y.Index
                          select new
                           {
                               Index = x.Index,
                               Date = x.TradingDay,
                               Value = x.Price,
                               Volume = y.Volume
                           };

            dataGridView1.DataSource = kapcsolt.ToList();
        }


        private void CreatePortfolio()
        {
            Portfolio.Add(new PortfolioItem() { Index = "OTP", Volume = 10 });
            Portfolio.Add(new PortfolioItem() { Index = "ZWACK", Volume = 10 });
            Portfolio.Add(new PortfolioItem() { Index = "ELMU", Volume = 10 });

            dataGridView2.DataSource = Portfolio;
        }

        private decimal GetPortfolioValue(DateTime date)
        {
            decimal value = 0;

            foreach (var item in Portfolio)
            {
                var last = (from x in Ticks
                            where item.Index == x.Index.Trim()
                            && date <= x.TradingDay
                            select x
                            ).First();

                value += (decimal)last.Price * item.Volume;
            }
            return value;
        }
    }
}
