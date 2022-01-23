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
        public IList<Cell> cells { get; set; }
    }

    public class Cell
    {
        public string type { get; set; } // standard, block
        public int number { get; set; }
    }

}
