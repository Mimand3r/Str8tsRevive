using Newtonsoft.Json;
using Str8tsGenerationProject.JSON;
using Str8tsGenerationProject.Pages.Controls;
using Str8tsGenerator;
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

        private async void GenerateLevelKlicked(object sender, EventArgs e)
        {
            var count = Convert.ToInt32(countBox.Value);
            var base_file_name = fileNameBox.Text;

            if (String.IsNullOrEmpty(base_file_name))
            {
                MessageBox.Show("Bitte einen File Namen angeben");
                return;
            }

            // Show Folder selection Dialog

            var folderDialog = new FolderBrowserDialog();
            folderDialog.SelectedPath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..");
            var dialogResult = folderDialog.ShowDialog();

            if (dialogResult != DialogResult.OK || string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
            {
                MessageBox.Show("Es muss ein Pfad gewählt werden unter dem die Dokumente gespeichert werden");
                return;
            }

            // Show progress popup

            var progressPopup = new ProgressDialog();
            progressPopup.Left = (this.Width - progressPopup.Width) / 2;
            progressPopup.Top = (this.Height - progressPopup.Height) / 2;
            this.Controls.Add(progressPopup);
            progressPopup.BringToFront();

            // Create Levels

            for (int counter = 1; counter <= count; counter++)
            {
                progressPopup.showMainText($"Datei {counter}/{count} wird erstellt.");
                await Task.Run(() =>
                {
                    var generated_level = Generator.GenerateLevel();
                    var jsonString = JsonConvert.SerializeObject(generated_level);
                    var potential_filename = "";
                    var nameCounter = 0;
                    var path = "";
                    do
                    {
                        nameCounter++;
                        potential_filename = base_file_name + "_" + nameCounter.ToString("D3") + ".json";
                        path = Path.GetFullPath(folderDialog.SelectedPath + "\\" + potential_filename);
                    } while (File.Exists(path));
                    File.WriteAllText(path, jsonString);
                });
            }

            this.Controls.Remove(progressPopup);
            MessageBox.Show($"Die Erstellung von {count} Levels war erfolgreich. Der Prozess hat {progressPopup.seconds} Sekunden gedauert.");

        }
    }
}
