using Str8tsGenerationProject.JSON;
using Str8tsGenerationProject.SolvingAlgorithm.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Str8tsGenerationProject.SolvingAlgorithm
{
    public static class SolvingUtils
    {
        public static void CreateCleanStraightsLayout(this SolverBoard board)
        {

            board.horizontal_str8tes = new List<Str8te>();
            board.vertical_str8tes = new List<Str8te>();

            for (int i = 0; i < board.size; i++)
            {
                var rowCells = board.Cells.Where(x => x.row_pos == i).ToList();
                var strats = ExtractStr8tsFromRowOrCol(rowCells, i, Str8teType.horizontal);
                board.horizontal_str8tes.AddRange(strats);
            }

            for (int i = 0; i < board.size; i++)
            {
                var collCells = board.Cells.Where(x => x.col_pos == i).ToList();
                var strats = ExtractStr8tsFromRowOrCol(collCells, i, Str8teType.vertical);
                board.vertical_str8tes.AddRange(strats);
            }

            List<Str8te> ExtractStr8tsFromRowOrCol(List<SolverCell> cells, int index, Str8teType type)
            {
                var block_values = cells.Where(x => x.isBlock && x.value > 0).Select(x => x.value).ToList(); // check for block-values, those go into all generated strates as forbidden types

                var newStrates = new List<Str8te>();
                var temp_cell_speicher = new List<SolverCell>();
                foreach (var cell in cells)
                {
                    if (cell.isBlock)
                    {
                        if (temp_cell_speicher.Count == 0) continue; // tritt auf wenn erstes Element block ist
                        var newStrate_ = new Str8te
                        {
                            index = index,
                            row_start = temp_cell_speicher[0].row_pos,
                            col_start = temp_cell_speicher[0].col_pos,
                            str8teType = type,
                            length = temp_cell_speicher.Count,
                            AlreadyIncludes = cells.Where(x => x.value > 0).Select(x => x.value).ToList(),
                        };
                        newStrate_.Cells.AddRange(temp_cell_speicher);
                        newStrate_.CannotInclude.AddRange(block_values);
                        newStrates.Add(newStrate_);
                        temp_cell_speicher.Clear();
                    }
                    else
                    {
                        temp_cell_speicher.Add(cell);
                    }
                }

                // Nach der Vorschleife kann noch eine Str8te übrig sein
                if (temp_cell_speicher.Count > 0)
                {
                    var newStrate_ = new Str8te
                    {
                        index = index,
                        row_start = temp_cell_speicher[0].row_pos,
                        col_start = temp_cell_speicher[0].col_pos,
                        str8teType = type,
                        length = temp_cell_speicher.Count,
                        AlreadyIncludes = cells.Where(x => x.value > 0).Select(x => x.value).ToList(),
                    };
                    newStrate_.Cells.AddRange(temp_cell_speicher);
                    newStrate_.CannotInclude.AddRange(block_values);
                    newStrates.Add(newStrate_);
                    temp_cell_speicher.Clear();
                }

                // Nun müssen die CannotInclude Values geupdated werden wannimmer eine Str8te ein Already Includes entry besitzt
                var strates_with_already_include_entrys = newStrates.Where(x => x.AlreadyIncludes.Count > 0).ToList();

                foreach (var strate_with_already_include_entrys in strates_with_already_include_entrys)
                {
                    var sisterStr8tes = newStrates.Where(x => x != strate_with_already_include_entrys).ToList();
                    sisterStr8tes.ForEach(sisterStr8te =>
                    {
                        sisterStr8te.CannotInclude.AddRange(strate_with_already_include_entrys.AlreadyIncludes);
                        sisterStr8te.CannotInclude = sisterStr8te.CannotInclude.Distinct().ToList();
                    });
                }

                // Die Initial Possibilities ergeben sich ausschließlich über die Länge der Str8te relativ zu BoardSize
                foreach (var newStrate in newStrates)
                {
                    var length = newStrate.length;
                    var board_size = board.size;

                    for (int i = 0; i <= board_size - length; i++)
                    {
                        newStrate.Possibilities.Add(new Range(i + 1, i + length));
                    }
                }

                return newStrates;

            }
        }

        public static void RecalculateAllStr8tePossibilities(this SolverBoard board)
        {
            // Possibilities werden immer Row/Col Weise kalkuliert da sister str8te längen eine große rolle für die ranges spielen
            for (int i = 0; i < board.size; i++)
            {
                var row_strates = board.horizontal_str8tes.Where(x => x.index == i).ToList();
                var anzahl_blocks_1 = board.Cells.Where(x => x.col_pos == i).ToList().Count;
                calcAllStratePossibilitiesForRowCol(row_strates, anzahl_blocks_1, board.size);

                var col_strates = board.vertical_str8tes.Where(x => x.index == i).ToList();
                var anzahl_blocks_2 = board.Cells.Where(x => x.row_pos == i).ToList().Count;
                calcAllStratePossibilitiesForRowCol(col_strates, anzahl_blocks_2, board.size);
            }
        }

        // Diese Funktion soll für jede str8te die Anzahl an Possibilities setzen
        public static void calcAllStratePossibilitiesForRowCol(List<Str8te> sibblingStrates, int anzahl_blocks, int boardSize)
        {
            List<Str8te> readyStrates = new List<Str8te>();
            List<Str8te> unreadyStrates = new List<Str8te>();
            unreadyStrates.AddRange(sibblingStrates);

            while (unreadyStrates.Count > 0)
            {
                var maxLength = unreadyStrates.Max(x => x.length);
                var longestUnreadyStrate = unreadyStrates.Find(x => x.length == maxLength);

                var anzahlMoeglichkeiten = boardSize - maxLength + 1; // Diese Funktion füllt die Ranges nach dem Prinzip: je kleiner die strate relativ zu dem Platz desto mehr möglichkeiten gibt es

                longestUnreadyStrate.Possibilities.Clear();

                for (int i = 1; i <= anzahlMoeglichkeiten; i++)
                {
                    var newPotentialRange = new Range(i, i + longestUnreadyStrate.length - 1);
                    var includesForbiddenValues = longestUnreadyStrate.CannotInclude.Any(x => newPotentialRange.isInRange(x));
                    if (includesForbiddenValues)  // Never add Range elements which includes a forbidden value
                        continue;
                    var includesAllRequiredElements = longestUnreadyStrate.AlreadyIncludes.All(x => newPotentialRange.isInRange(x));
                    if (!includesAllRequiredElements) // Never add Ranges which do not include full alreadyInclude List
                        continue;

                    longestUnreadyStrate.Possibilities.Add(newPotentialRange);
                }

                // Checke ob die neuen Ranges, neue must include Options erzeugt hat
                for (int i = 1; i <= 9; i++)
                {
                    var isInAllRanges = longestUnreadyStrate.Possibilities.All(x => x.isInRange(i));
                    var isInAlreadyIncludes = longestUnreadyStrate.AlreadyIncludes.Contains(i);
                    var isInMustInclude = longestUnreadyStrate.MustInclude.Contains(i);
                    if (isInAllRanges && !isInAlreadyIncludes && !isInMustInclude)
                    {
                        longestUnreadyStrate.MustInclude.Add(i);
                        longestUnreadyStrate.MustInclude.Distinct().ToList();
                        // Wann immer ein must-include entry geaddet wurde, so müssen alle sister-str8tes den entry als cannot include bekommen und die Row-calculation muss neu starten
                        var otherSibblings = sibblingStrates.Where(x => x != longestUnreadyStrate).ToList();
                        otherSibblings.ForEach(x => x.CannotInclude.Add(i));
                        calcAllStratePossibilitiesForRowCol(sibblingStrates, anzahl_blocks, boardSize);
                        return;
                    }
                }

                readyStrates.Add(longestUnreadyStrate);
                unreadyStrates.Remove(longestUnreadyStrate);
            }
        }
    }
}
