using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Str8tsGenerationProject
{
    public partial class Zelle : UserControl
    {
        public Zelle()
        {
            InitializeComponent();
        }

        [Category("Zelle"), Description("A property that controls the wossname")]
        public int Zahl
        {
            get { return int.Parse(label1.Text); }
            set { label1.Text = value.ToString(); }
        }
    }
}
