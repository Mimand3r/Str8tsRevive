using Newtonsoft.Json;
using Str8tsGenerationProject.JSON;
using Str8tsGenerationProject.Pages.Controls;
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

namespace Str8tsGenerationProject.Pages
{
    public partial class Page_Import_And_Display : UserControl
    {
        public Page_Import_And_Display()
        {
            InitializeComponent();
        }

        private JSONBoard jsonBoard = null;

        private void button_import_and_draw_clicked(object sender, EventArgs e)
        {
            // Dialog
            var openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "JSON files (*.json)|*.json;";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            // Import Data
            var selectedFile = openFileDialog1.FileName;
            if (!File.Exists(selectedFile)) return;
            var title = selectedFile.Split('\\').Last();
            var fileData = File.ReadAllText(selectedFile);
            this.jsonBoard = JsonConvert.DeserializeObject<JSONBoard>(fileData);
            file_label.Text = title;

            // Draw Board
            var static_board = new Static_Board(this.jsonBoard);
            static_board.Dock = DockStyle.Top;
            panel1.Controls.Clear();
            panel1.Controls.Add(static_board);
        }
    }
}
