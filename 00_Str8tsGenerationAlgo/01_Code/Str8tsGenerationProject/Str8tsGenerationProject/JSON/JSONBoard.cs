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
        public IList<JSONBoardCell> cells { get; set; }
    }

    public class JSONBoardCell
    {
        public string type { get; set; } // standard, block
        public int number { get; set; }
    }

}
