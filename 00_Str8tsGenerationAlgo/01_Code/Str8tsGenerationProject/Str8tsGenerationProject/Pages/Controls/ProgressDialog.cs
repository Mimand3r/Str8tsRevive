using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Str8tsGenerationProject.Pages.Controls
{
    public partial class ProgressDialog : UserControl
    {
        public ProgressDialog()
        {
            InitializeComponent();

            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Tick += (s, e) =>
            {
                seconds += 1;
                timer.Text = seconds.ToString();
            };
            _timer.Start();
        }

        private Timer _timer;
        public int seconds = 0;

        public void showMainText(string text)
        {
            mainText.Text = text;
        }

    }
}
