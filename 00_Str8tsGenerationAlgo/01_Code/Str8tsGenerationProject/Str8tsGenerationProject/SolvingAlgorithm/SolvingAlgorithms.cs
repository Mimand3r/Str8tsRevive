using Str8tsGenerationProject.JSON;
using Str8tsGenerationProject.SolvingAlgorithm.Exceptions;
using Str8tsGenerationProject.SolvingAlgorithm.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Str8tsGenerationProject.SolvingAlgorithm
{
    public static class SolvingAlgorithms
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

            // Checke ob eine der Str8tes keine Possibilities hat, wenn ja, so existiert keine Lösung für das Board
            var zeroPossibilitiesExist = sibblingStrates.Any(x => x.Possibilities.Count == 0);
            if (zeroPossibilitiesExist)
                throw new NoSolutionException();
        }

        public static void RecalculateAllCellPossibilities(this SolverBoard board)
        {
            /// Iterate over every unsolved Cell.
            /// Check the both str8tes the cell is part of, exclude CannotInclude values from cell possiblities & also exclude alreadyincludes
            /// Throw eine no solution exception wenn eine der Zellen keine Possibilities mehr besitzt

            foreach (var cell in board.Cells)
            {
                // is cell solved so collapse possibilities
                if (cell.isSolved)
                {
                    cell.possibleValues.Clear();
                    continue;
                }

                var cell_str8te_hor = board.horizontal_str8tes.Find(x => x.Cells.Contains(cell));
                var cell_str8te_vert = board.vertical_str8tes.Find(x => x.Cells.Contains(cell));
                var cell_str8tes = new List<Str8te> { cell_str8te_hor, cell_str8te_vert };

                // Exclude every forbidden Value from Cell
                var cannot_includes = cell_str8tes.SelectMany(x => x.CannotInclude).Distinct().ToList();
                var already_includes = cell_str8tes.SelectMany(x => x.AlreadyIncludes).Distinct().ToList();
                var forbiddenValues = new List<int>();
                forbiddenValues.AddRange(cannot_includes);
                forbiddenValues.AddRange(already_includes);
                forbiddenValues = forbiddenValues.Distinct().ToList();
                foreach (var forbidden_value in forbiddenValues)
                {
                    cell.possibleValues.Remove(forbidden_value);
                }

                // Checke ob die verbleibenden Cell Options in der Possible Range beider Str8tes liegen. Ansonsten remove die cell-option
                var cell_possible_values = new List<int>();
                cell_possible_values.AddRange(cell.possibleValues);
                foreach (var left_cell_possibility in cell_possible_values)
                {
                    var isInRangeForBoth = cell_str8tes.All(x => x.Possibilities.Any(p => p.isInRange(left_cell_possibility)));

                    if (!isInRangeForBoth)
                        cell.possibleValues.Remove(left_cell_possibility);
                }

                // If cell has no possibilities Left board has no solution
                if (cell.possibleValues.Count == 0)
                    throw new NoSolutionException();

            }
        }

        // Function returns true if paare triggered mustInclude or cannotInclude entrys in any str8te
        public static bool CollapseCellOptionsIfPaarelementeExist(this SolverBoard board)
        {
            var needs_solving_step_restart = false;

            for (int i = 0; i < board.size; i++)
            {
                var row_str8tes = board.horizontal_str8tes.Where(x => x.Cells[0].col_pos == i).ToList();
                var needsRefresh = CollapseCellOptionsIfPaarelementeExistForRowOrCol(row_str8tes);
                if (needsRefresh)
                    needs_solving_step_restart = true;
            }

            for (int i = 0; i < board.size; i++)
            {
                var col_str8tes = board.vertical_str8tes.Where(x => x.Cells[0].row_pos == i).ToList();
                var needsRefresh = CollapseCellOptionsIfPaarelementeExistForRowOrCol(col_str8tes);
                if (needsRefresh)
                    needs_solving_step_restart = true;
            }

            return needs_solving_step_restart;
        }

        private static bool CollapseCellOptionsIfPaarelementeExistForRowOrCol(List<Str8te> str8tes)
        {
            List<SolverCell> paare = new List<SolverCell>();
            List<SolverCell> cells = str8tes.SelectMany(x => x.Cells).ToList();

            var made_entry = false; // decides wether refresh must happen

            for (int i = 0; i < cells.Count; i++)
            {
                var current_cell = cells[i];
                var other_cells = cells.Where(x => x != current_cell).ToList();
                var paar_cells_other = other_cells.Where(x => x.possibleValues.IntListsAreEqual(current_cell.possibleValues)).ToList();

                var paare_count = paar_cells_other.Count + 1;

                if (paare_count == 1)
                    continue;

                if (paare_count != current_cell.possibleValues.Count)
                    continue;

                /// Nun wurden paare detected
                /// Alle anderen Zellen in der Row/col bekommen nun die Paare als Option removed
                /// Dropped so eine der Zellen auf possibility länge 0 so wird eine NoSolution Exception gethrowed werden
                /// Weiterhin muss geprüft werden ob alle Paare in einer Str8te liegen. Wenn ja so bekommt die Str8te die values als must include options sofern nicht bereits existent
                /// Weiterhin muss geprüft werden ob in der Row/Col Str8tes existieren die kein Paar enthalten. Wenn ja so bekommen die die values als cannot include sofern nicht bereits existent
                /// wann immer es must-include oder cannot-include Änderungen gab, so muss der gesamte solving step wiederholt werden

                var non_paar_cells = other_cells.Where(x => !paar_cells_other.Contains(x));
                var all_paar_cells = new List<SolverCell>();
                all_paar_cells.Add(current_cell);
                all_paar_cells.AddRange(paar_cells_other);

                // Remove alle Paar Possibilities
                foreach (var item in non_paar_cells)
                {
                    item.possibleValues.RemoveAll(x => current_cell.possibleValues.Contains(x));

                    if (item.possibleValues.Count == 0)
                        throw new NoSolutionException();
                }

                // Liegen alle paare in einer Str8te?
                var paare_str8tes = str8tes.FindAll(x => x.Cells.Any(c => all_paar_cells.Contains(c)));
                if (paare_str8tes.Count == 1)
                {
                    // Die Str8te muss die PaarPossibilities als must include eintrage adden und sister str8tes bekommen einen cannot include eintrag, ist so tatsächlich ein value hinzugekommen, so muss der ganze solving step wiederholt werden
                    var paar_ints = all_paar_cells[0].possibleValues;
                    var other_str8tes = str8tes.Where(x => x != paare_str8tes[0]).ToList();
                    foreach (var paar_int in paar_ints)
                    {
                        if (!paare_str8tes[0].AlreadyIncludes.Contains(paar_int))
                        {
                            paare_str8tes[0].AlreadyIncludes.Add(paar_int);
                            made_entry = true;
                        }

                        other_str8tes.ForEach(other_str8te => {
                            if (!other_str8te.CannotInclude.Contains(paar_int))
                            {
                                other_str8te.CannotInclude.Add(paar_int);
                                made_entry = true;
                            }
                        });
                    }

                }

                // Gibt es in der Row Str8tes ohne ein Paar Element -> für die sind alle PaarNumbers cannotinclude

                var str8tes_ohne_paar = str8tes.Where(x => !paare_str8tes.Contains(x)).ToList();

                foreach (var str8te_ohne_paar in str8tes_ohne_paar)
                {
                    var paar_ints = all_paar_cells[0].possibleValues;

                    foreach (var paar_int in paar_ints)
                    {
                        if (!str8te_ohne_paar.CannotInclude.Contains(paar_int))
                        {
                            str8te_ohne_paar.CannotInclude.Add(paar_int);
                            made_entry = true;
                        }
                    }
                }

            }

            return made_entry;
        }

        public static void CollapseCellOptionsForMustIncludeHeroCells(this SolverBoard board)
        {
            var all_str8tes = new List<Str8te>();
            all_str8tes.AddRange(board.vertical_str8tes);
            all_str8tes.AddRange(board.horizontal_str8tes);

            foreach (var str8te in all_str8tes)
            {
                var all_cells = str8te.Cells;
                var must_includes = str8te.MustInclude;

                // gibt es must include entrys die nur eine zelle abdecken kann? wenn ja dann ist das eine hero Zelle 
                foreach (var must_include in must_includes)
                {
                    var possible_heros = all_cells.Where(x => x.possibleValues.Contains(must_include)).ToList();
                    if (possible_heros.Count == 0)
                        throw new NoSolutionException();

                    if (possible_heros.Count == 1)
                    {
                        var hero_zelle = possible_heros[0];
                        hero_zelle.possibleValues = new List<int> { must_include };
                    }
                }

            }
        }
    }
}
