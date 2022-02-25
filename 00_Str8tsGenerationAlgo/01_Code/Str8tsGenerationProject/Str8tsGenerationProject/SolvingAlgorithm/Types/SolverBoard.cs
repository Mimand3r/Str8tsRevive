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
        public SolverBoard(JSON.JSONBoard board)
        {
            this.size = board.size;
            var i = 0;
            this.Cells = board.cells.Select(x => new SolverCell(x, i++, board.size)).ToList();
        }

        private SolverBoard()
        {

        }

        public static SolverBoard createEmptyBoard()
        {
            // TODO
            return new SolverBoard();
        }

      

        public List<SolverCell> Cells { get; set; } = new List<SolverCell>();
        public int size { get; set; }
        public bool isUnsolved { get { return Cells.Any(x => !x.isSolved); } }

        public List<Str8te> RowStr8tes { get; set; }
        public List<Str8te> ColStr8tes { get; set; }

        public void CreateAllOptionsForAllUnfilledFields()
        {
            var unfilledCells = Cells.Where(x => !x.isSolved).ToList();
            unfilledCells.ForEach(x => x.possibleValues.AddRange(new[] {1, 2, 3, 4, 5, 6, 7, 8, 9}));
        }

        public void RemoveOptionsAccordingToRowCol()
        {
            foreach (var cell in Cells)
            {
                if (cell.isSolved) continue;
                var row_sibling_cells = Cells.Where(x => x != cell).Where(x => x.row == cell.row).ToList();
                var col_sibling_cells = Cells.Where(x => x != cell).Where(x => x.col == cell.col).ToList();

                var used_row_numbers = row_sibling_cells.Where(x => x.isSolved).Select(x => x.value).Distinct().ToList();
                var used_col_numbers = col_sibling_cells.Where(x => x.isSolved).Select(x => x.value).Distinct().ToList();

                var forbidden_numbers = used_row_numbers.Concat(used_col_numbers).Distinct().ToList();

                // Remove all Forbidden Numbers from
                foreach (var number in forbidden_numbers)
                {
                    cell.possibleValues.Remove(number);
                }

                if (cell.possibleValues.Count == 0)
                    throw new NoSolutionException();
            }
        }

        public void CreateCleanStraightsLayout()
        {
            // Create Row Str8ts

            var rowStrates = new List<Str8te>();

            for (int i = 0; i < this.size; i++)
            {
                var rowCells = Cells.Where(x => x.row == i).ToList();
                var strats = ExtractStr8tsFromRowOrCol(rowCells, i);
                rowStrates.AddRange(strats);
            }

            var collStrates = new List<Str8te>();

            // Create Col Str8ts
            for (int i = 0; i < this.size; i++)
            {
                var collCells = Cells.Where(x => x.col == i).ToList();
                var strats = ExtractStr8tsFromRowOrCol(collCells, i);
                collStrates.AddRange(strats);
            }

            // 

            this.RowStr8tes = rowStrates;
            this.ColStr8tes = collStrates;

            List<Str8te> ExtractStr8tsFromRowOrCol(List<SolverCell> cells, int index)
            {
                var block_values = cells.Where(x => x.isBlock && x.value > 0).Select(x => x.value).ToList(); // check for block-values, those go into all generated strates as forbidden types

                var newStrates = new List<Str8te>();
                var currentStrateCells = new List<SolverCell>();
                foreach (var cell in cells)
                {
                    if (cell.isBlock)
                    {
                        var newStrate_ = createNewStrate(currentStrateCells, block_values);
                        if (newStrate_ == null) continue;
                        newStrates.Add(newStrate_);
                        currentStrateCells.Clear();
                    }
                    else
                    {
                        currentStrateCells.Add(cell);
                    }
                }

                var newStrate = createNewStrate(currentStrateCells, block_values);
                if (newStrate != null)
                {
                    newStrates.Add(newStrate);
                    currentStrateCells.Clear();
                }

                var blockCount = cells.Where(x => x.isBlock).ToList().Count;
                setAllStratePossibilitiesForRow(newStrates, block_values, blockCount, cells.Count);

                return newStrates;

                Str8te createNewStrate(List<SolverCell> strateCells, List<int> forbiddenValues)
                {
                    if (strateCells.Count == 0) return null;

                    return new Str8te
                    {
                        index = index,
                        length = strateCells.Count,
                        Cells = strateCells,
                        CannotInclude = forbiddenValues,
                        AlreadyIncludes = cells.Where(x => x.value > 0).Select(x => x.value).ToList(),                       
                    };
                }

            }
        }

        public static void setAllStratePossibilitiesForRow(List<Str8te> sibblingStrates, List<int> forbiddenValues, int anzahl_blocks, int boardSize)
        {
            // Diese Funktion soll für jede str8te die Anzahl an Möglichkeiten setzen
            List<Str8te> readyStrates = new List<Str8te>();
            List<Str8te> unreadyStrates = new List<Str8te>();
            unreadyStrates.AddRange(sibblingStrates);

            while (unreadyStrates.Count > 0)
            {
                var maxLength = unreadyStrates.Max(x => x.length);
                var longestUnreadyStrate = unreadyStrates.Find(x => x.length == maxLength);

                // suche alle sibbling strates ab und adde deren values zu den forbidden_values für die strate
                var otherSibblings = sibblingStrates.Where(x => x != longestUnreadyStrate).ToList();
                var sibblingSetValues = 
                var forbiddenForThisStrate = new List<int>().AddRange(forbiddenValues).AddRange(otherSibblings.Cells)

                // Hat die Str8te bereits gefüllte values? dann setzt als already includes
                longestUnreadyStrate.AlreadyIncludes = longestUnreadyStrate.Cells.Where(x => x.isSolved).Select(x => x.value).ToList();

                // Set Possiblities for longest unready strate
                var anzahlMoeglichkeiten = boardSize - maxLength + 1;

                for (int i = 1; i <= anzahlMoeglichkeiten; i++)
                {
                    // Never add Range elements which includes a forbidden value
                    var newPotentialRange = new Range(i, i + longestUnreadyStrate.length - 1);
                    var includesForbiddenValues = forbiddenValues.Any(x => newPotentialRange.isInRange(x));
                    if (includesForbiddenValues)
                        continue;
                    // Never add Ranges which do not include full alreadyInclude List
                    var includesAllRequiredElements = longestUnreadyStrate.AlreadyIncludes.All(x => newPotentialRange.isInRange(x));
                    if (!includesAllRequiredElements)
                        continue;

                    // Diese Funktion füllt die Ranges nach dem Prinzip: je kleiner die strate relativ zu dem Platz desto mehr möglichkeiten gibt es
                    longestUnreadyStrate.Possibilities.Ranges.Add(new Range(i, i + longestUnreadyStrate.length - 1));
                }

                // Update Must Include According to the defined Ranges
                for (int i = 1; i <= 9; i++)
                {
                    var isInAllRanges = longestUnreadyStrate.Possibilities.Ranges.All(x => x.isInRange(i));
                    var alreadyIncludes = longestUnreadyStrate.AlreadyIncludes.Contains(i);
                    if (isInAllRanges && !alreadyIncludes)
                    {
                        longestUnreadyStrate.MustInclude.Add(i);
                        longestUnreadyStrate.MustInclude.Distinct().ToList();
                        forbiddenValues.Add(i);
                    }

                }

                readyStrates.Add(longestUnreadyStrate);
                unreadyStrates.Remove(longestUnreadyStrate);

                forbiddenValues.AddRange(longestUnreadyStrate.AlreadyIncludes);
                forbiddenValues.AddRange(longestUnreadyStrate.MustInclude);
                forbiddenValues = forbiddenValues.Distinct().ToList();
            }


        }
    }

    public class SolverCell
    {
        public SolverCell(JSON.JSONBoardCell jsonCell, int index, int board_size)
        {
            this.index = index;
            this.value = jsonCell.number;
            this.isBlock = jsonCell.type == "block";
            this.isSolved = this.isBlock || this.value > 0;
            this.row = index / board_size;
            this.col = index % board_size;
        }

        public int index { get; set; }
        public int row { get; set; }
        public int col { get; set; }
        public int value { get; set; }
        public bool isSolved { get; set; }
        public List<int> possibleValues { get; set; } = new List<int>();
        public bool isBlock { get; set; }
    }

    public class Str8te
    {
        public int index { get; set; }
        public int length { get; set; }
        public List<int> MustInclude { get; set; } = new List<int>(); 
        public List<int> AlreadyIncludes { get; set; } = new List<int>();
        public List<int> CannotInclude { get; set; } = new List<int>(); 
        public List<SolverCell> Cells { get; set; } = new List<SolverCell>();
        public Possibilities Possibilities { get; set; } = new Possibilities();
    }

    public class Possibilities
    {
        public List<Range> Ranges { get; set; } = new List<Range>();
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
    }
}
