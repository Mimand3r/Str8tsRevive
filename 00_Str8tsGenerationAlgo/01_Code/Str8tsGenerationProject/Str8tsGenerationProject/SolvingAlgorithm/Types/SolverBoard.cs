using Str8tsGenerationProject.SolvingAlgorithm.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Str8tsGenerationProject.SolvingAlgorithm.Types
{
    /// <summary>
    /// Base Cell Definition in Solver Context. Easier to Handle for Solver.
    /// Is close to JSON Board Cell and can be created from JSON Board 
    /// </summary>
    public class SolverBoard
    {
        
        // Jedes Board besteht aus Cells & Strates. Jede einzelne Cell ist Teil von 2 Str8tes
        public List<SolverCell> Cells { get; set; } = new List<SolverCell>();
        public List<Str8te> horizontal_str8tes { get; set; }
        public List<Str8te> vertical_str8tes { get; set; }
        public int size { get; set; }
        public bool isUnsolved { get { return Cells.Any(x => !x.isSolved); } }


        private SolverBoard(JSON.JSONBoard board)
        {
            this.size = board.size;
            var i = 0;
            this.Cells = board.cells.Select(x => new SolverCell(x, i++, board.size)).ToList();
        }

        public static SolverBoard createEmptyBoard(JSON.JSONBoard board)
        {
            return new SolverBoard(board);
        }
    }
}
