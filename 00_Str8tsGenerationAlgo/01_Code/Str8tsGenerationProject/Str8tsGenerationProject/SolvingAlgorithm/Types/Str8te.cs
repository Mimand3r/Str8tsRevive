using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Str8tsGenerationProject.SolvingAlgorithm.Types
{
    public class Str8te
    {
        public int index { get; set; }
        public int row_start { get; set; }
        public int col_start { get; set; }
        public int length { get; set; }
        public Str8teType str8teType { get; set; }
        public List<SolverCell> Cells { get; set; } = new List<SolverCell>();
        public List<int> MustInclude { get; set; } = new List<int>();
        public List<int> AlreadyIncludes { get; set; } = new List<int>();
        public List<int> CannotInclude { get; set; } = new List<int>();
        public List<Range> Possibilities { get; set; } = new List<Range>();
        public List<Range> ForbiddenPossibilities { get; set; } = new List<Range>(); // due to cross-str8tes

        public bool isSolved => this.Cells.All(x => x.isSolved);
    }

    public enum Str8teType
    {
        horizontal,
        vertical
    }


    public class Range
    {
        public int Start { get; set; }
        public int End { get; set; }

        public Range(int start, int end)
        {
            this.Start = start;
            this.End = end;
        }

        public bool isInRange(int i)
        {
            return i >= this.Start && i <= this.End;
        }

        public bool isFromTo(int start, int end) => start == this.Start && end == this.End;

        public bool isSimilarTo(Range other)
        {
            if (other == this)
                return true;

            if (this.Start == other.Start && this.End == other.End)
                return true;

            return false;
        }

        public List<int> getRangeNumberList()
        {
            var output = new List<int>();

            for (int i = this.Start; i <= this.End; i++)
            {
                output.Add(i);
            }

            return output;
        }
    }
}
