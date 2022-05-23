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
        public JSONBoard SolvedBoard { get; set; } = null;
    }

    public enum ResultType
    {
        Success,
        NoSolution,
        MultipleSolutions
    }
}
