using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Str8tsGenerationProject.SolvingAlgorithm.Types
{
    public class SolverCell
    {
        public SolverCell(JSON.JSONBoardCell jsonCell, int index, int board_size)
        {
            this.index = index;
            this.value = jsonCell.number;
            this.isBlock = jsonCell.type == "block";
            this.isSolved = this.isBlock || this.value > 0;
            this.row_pos = index / board_size;
            this.col_pos = index % board_size;

            if (!isSolved)
                possibleValues.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        }

        public int index { get; set; }
        public int row_pos { get; set; }
        public int col_pos { get; set; }
        public int value { get; set; }
        public bool isSolved { get; set; }
        public List<int> possibleValues { get; set; } = new List<int>();
        public bool isBlock { get; set; }
    }
}
