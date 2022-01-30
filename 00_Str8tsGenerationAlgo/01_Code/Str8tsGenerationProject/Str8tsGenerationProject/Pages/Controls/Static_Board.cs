using Str8tsGenerationProject.JSON;
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
    public partial class Static_Board : UserControl
    {
        public Static_Board(JSONBoard board)
        {
            InitializeComponent();
            this.board = board;

            this.Width = 40 * board.size;
            this.Height = 40 * board.size;

            draw_cell_grid();
        }

        private JSONBoard board;

        private void draw_cell_grid()
        {
            for (int i = 0; i < board.size; i++)
            {
                for (int j = 0; j < board.size; j++)
                {
                    var newEl = new Zelle(new Point(j * 40, i * 40), board.cells[i * board.size + j]);
                    this.Controls.Add(newEl);
                }
            }
        }

    }
}
