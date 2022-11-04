using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Factory_patterns.Entities;

namespace Factory_patterns
{
    public partial class Form1 : Form
    {
        List<Ball> _balls = new List<Ball>();
        private BallFactory _factory;

        public BallFactory Factory
        {
            get { return _factory; }
            set { _factory = value; }
        }


        public Form1()
        {
            InitializeComponent();
            Factory = new BallFactory();

            createTimer.Start();
            conveyorTimer.Start();
        }

        private void createTimer_Tick(object sender, EventArgs e)
        {
            Ball ball = _factory.CreateNew();

            ball.Left -= ball.Width;

            _balls.Add(ball);
            mainPanel.Controls.Add(ball);
        }

        private void conveyorTimer_Tick(object sender, EventArgs e)
        {
            var maxPosition = 0;

            foreach (var ball in _balls)
            {
                ball.MoveBall();

                if (ball.Left > maxPosition)
                {
                    maxPosition = ball.Left;
                }
            }

            if (maxPosition > 1000)
            {
                var oldest_ball = _balls[0];

                _balls.Remove(oldest_ball);
                mainPanel.Controls.Remove(oldest_ball);
            }
        }
    }
}
