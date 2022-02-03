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
    public partial class Editable_Board : UserControl
    {
        public Editable_Board(int size)
        {
            InitializeComponent();

            this.Width = 40 * size;
            this.Height = 40 * size;

            draw_cell_grid(size);
        }

        public List<Zelle_Editable> zellen { get; private set; } = new List<Zelle_Editable>();

        private void draw_cell_grid(int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var newEl = new Zelle_Editable(new Point(j * 40, i * 40));
                    zellen.Add(newEl);
                    this.Controls.Add(newEl);
                }
            }
        }
    }
}
