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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

         
        }

        private String selectedFile = null;

        private void button_import_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "JSON files (*.json)|*.json;";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                selectedFile = openFileDialog1.FileName;
                label1.Text = selectedFile.Split('\\').Last();
            }
        }

        private void button_draw_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFile)) return;

            // Create 9x9 Cell Array
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    var newEl = new Zelle();
                    newEl.Location = new Point(i * 40, j * 40);
                    this.Controls.Add(newEl);
                }
            }
        }
    }
}
