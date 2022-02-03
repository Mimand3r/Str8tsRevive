using Newtonsoft.Json;
using Str8tsGenerationProject.JSON;
using Str8tsGenerationProject.Pages;
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
            
            tab_control.TabPages.Clear();
            
            var page1 = new TabPage("Import & Display");
            var ImportAndDisplayPage = new Page_Import_And_Display();
            ImportAndDisplayPage.Parent = page1;

            var page2 = new TabPage("Manual Board Creation");
            var manual_board_creation_page = new Page_Manual_Board_Creation();
            manual_board_creation_page.Parent = page2;

            var page3 = new TabPage("Board Solver");
            var solve_board_page = new Page_Solve_Board();
            solve_board_page.Parent = page3;

            tab_control.TabPages.Add(page1);
            tab_control.TabPages.Add(page2);
            tab_control.TabPages.Add(page3);
        }
    }
}
