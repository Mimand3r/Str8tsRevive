using Newtonsoft.Json;
using Str8tsGenerationProject.JSON;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

        private JSONBoard jsonBoard = null;

        private void button_import_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "JSON files (*.json)|*.json;";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            var selectedFile = openFileDialog1.FileName;
            if (!File.Exists(selectedFile)) return;
            
            var title = selectedFile.Split('\\').Last();
            var fileData = File.ReadAllText(selectedFile);
            this.jsonBoard = JsonConvert.DeserializeObject<JSONBoard>(fileData);
            label1.Text = title;
        }

        private void button_draw_Click(object sender, EventArgs e)
        {
            if (jsonBoard == null) return;
            if (jsonBoard.cells.Count != jsonBoard.size * jsonBoard.size) return;

            for (int i = 0; i < jsonBoard.size; i++)
            {
                for (int j = 0; j < jsonBoard.size; j++)
                {
                    var newEl = new Zelle(new Point(j * 40, i * 40), jsonBoard.cells[i*jsonBoard.size + j]);
                    this.Controls.Add(newEl);
                }
            }
        }
    }
}
