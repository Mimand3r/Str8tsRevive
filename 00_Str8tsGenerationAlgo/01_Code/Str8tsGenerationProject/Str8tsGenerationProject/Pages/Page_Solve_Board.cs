using Newtonsoft.Json;
using Str8tsGenerationProject.JSON;
using Str8tsGenerationProject.Pages.Controls;
using Str8tsGenerationProject.SolvingAlgorithm.Types;
using Str8tsGenerator.Types;
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
    public partial class Page_Solve_Board : UserControl
    {
        public Page_Solve_Board()
        {
            InitializeComponent();
        }

        private JSONBoard jsonBoard = null;

        private void button_import_and_display_Click(object sender, EventArgs e)
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
            
            // Unterstützt sowohl generator json files (dann wird emptyboard displayed) als auch manuell erzeugte JSONBoards
            var isGeneratorJSON = fileData.Contains("EmptyBoard");
            if (isGeneratorJSON)
                this.jsonBoard = JsonConvert.DeserializeObject<GenerationResult>(fileData).EmptyBoard;
            else
                this.jsonBoard = JsonConvert.DeserializeObject<JSONBoard>(fileData);

            file_label.Text = title;

            // Draw Board
            var static_board = new Static_Board(this.jsonBoard);
            static_board.Dock = DockStyle.Top;
            panel1.Controls.Clear();
            panel1.Controls.Add(static_board);
        }

        private void button_solve_Click(object sender, EventArgs e)
        {
            var solvingResult = SolvingAlgorithm.MainSolver.SolveBoard(jsonBoard);

            if (solvingResult.ResultType == ResultType.NoSolution)
            {
                MessageBox.Show("Keine Lösung");
            }
            else if (solvingResult.ResultType == ResultType.MultipleSolutions)
            {
                MessageBox.Show("Mehrere Lösungen");
            }
            else
            {
                string json = JsonConvert.SerializeObject(solvingResult.SolvedBoard);
                // Show Save File Dialog
                var saveFileDialog = new SaveFileDialog();

                saveFileDialog.Filter = "JSON files (*.json)|*.json;";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

                var filename = saveFileDialog.FileName;

                File.WriteAllText(filename, json);
            }
        }
    }
}
