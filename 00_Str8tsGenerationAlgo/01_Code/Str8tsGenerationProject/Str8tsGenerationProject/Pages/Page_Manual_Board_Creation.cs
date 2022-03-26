using Newtonsoft.Json;
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
    public partial class Page_Manual_Board_Creation : UserControl
    {
        public Page_Manual_Board_Creation()
        {
            InitializeComponent();

            // Draw Board
            editable_board = new Editable_Board(9);
            editable_board.Dock = DockStyle.Top;
            panel1.Controls.Clear();
            panel1.Controls.Add(editable_board);
        }

        Editable_Board editable_board;

        private void store_as_json_clicked(object sender, EventArgs e)
        {
            // Gather Data
            var jsonboard = new JSON.JSONBoard
            {
                size = Convert.ToInt32(Math.Sqrt(editable_board.zellen.Count))
            };

            foreach (var zelle in editable_board.zellen)
            {
                jsonboard.cells.Add(new JSON.JSONBoardCell
                {
                    number = zelle.Number,
                    type = !zelle.IsBlock? "standard" : "block"
                });
            }

            var jsonString = JsonConvert.SerializeObject(jsonboard);

            // Show Save File Dialog
            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "JSON files (*.json)|*.json;";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            var filename = saveFileDialog.FileName;

            File.WriteAllText(filename, jsonString);

        }

        private void clear_clicked(object sender, EventArgs e)
        {
            // Draw Board
            editable_board = new Editable_Board(9);
            editable_board.Dock = DockStyle.Top;
            panel1.Controls.Clear();
            panel1.Controls.Add(editable_board);
        }
    }
}
