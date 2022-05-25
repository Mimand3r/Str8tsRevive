using Str8tsGenerationProject.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Str8tsGenerationProject.SolvingAlgorithm.Types
{
    public class SolvingResult
    {
        public ResultType ResultType { get; set; }

        /// <summary>
        /// Wird nur im Falle einer Success returned
        /// </summary>
        public JSONBoard SolvedBoard { get; set; } = null;

        /// <summary>
        /// Wird nur im Falle einer MultipleSolution returned
        /// </summary>
        public SolverBoard UnsolvedBoard { get; set; } = null;
    }

    public enum ResultType
    {
        Success,
        NoSolution,
        MultipleSolutions
    }
}
