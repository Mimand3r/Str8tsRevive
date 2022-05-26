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

namespace Str8tsGenerationProject
{
    public partial class Zelle : UserControl
    {
        public Zelle(Point location, JSONBoardCell jsonBoardCell)
        {
            InitializeComponent();
            this.Location = location;
            this.jsonBoardCell = jsonBoardCell;
            setZahl(jsonBoardCell.number);
            setType(jsonBoardCell.type);
        }

        private JSONBoardCell jsonBoardCell;

        [Category("Zelle"), Description("A property that controls the wossname")]
        private void setZahl(int zahl)
        {
            if (zahl > 0)
                label1.Text = zahl.ToString();
            else
                label1.Text = string.Empty;
        }

        private void setType(string type)
        {
            if (type == "standard")
                panel1.BackColor = Color.White;
            else if (type == "block")
                panel1.BackColor = ColorTranslator.FromHtml("#888888");
            else
                throw new Exception("Fehler in JSON");
        }
    }
}
