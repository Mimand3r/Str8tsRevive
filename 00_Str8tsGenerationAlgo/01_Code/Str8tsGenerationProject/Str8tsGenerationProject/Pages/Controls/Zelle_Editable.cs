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
    public partial class Zelle_Editable : UserControl
    {
        public Zelle_Editable(Point location)
        {
            InitializeComponent();
            this.Location = location;
        }

        public int Number { get; set; } = 0;
        public bool IsBlock
        {
            get => this.BackColor != Color.White;
            set => this.BackColor = value? Color.Gray : Color.White;
        }

        private void mouse_right_klick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            IsBlock = !IsBlock;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var value = textBox1.Text;
            if (String.IsNullOrEmpty(value)) return;
            if (int.TryParse(value, out int res))
            {
                this.Number = res;
                return;
            }

            // dont allow non int values
            textBox1.Text = "0";
            this.Number = 0;
        }
    }
}
