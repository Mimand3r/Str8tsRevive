using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Str8tsGenerationProject.JSON
{
    public class JSONBoard
    {
        public int size { get; set; }
        public List<JSONBoardCell> cells { get; set; } = new List<JSONBoardCell>();
    }

    public class JSONBoardCell
    {
        public string type { get; set; } // standard, block
        public int number { get; set; }
    }

}
