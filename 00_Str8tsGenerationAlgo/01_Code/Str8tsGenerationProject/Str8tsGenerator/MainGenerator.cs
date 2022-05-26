﻿using Str8tsGenerationProject.JSON;
using Str8tsGenerationProject.SolvingAlgorithm;
using Str8tsGenerationProject.SolvingAlgorithm.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Str8tsGenerator
{
    public class Generator
    {
        public static JSONBoard GenerateLevel(int size = 9)
        {
            var random = new System.Random(Guid.NewGuid().GetHashCode());

            var newBoard = new JSONBoard { size = size };

            // Fill Blocks

            var max = 0.35;
            var min = 0.2;
            var block_wahrscheinlichkeit = random.NextDouble() * (max - min) + min;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var isBlock = random.NextDouble() <= block_wahrscheinlichkeit;

                    var newCell = new JSONBoardCell
                    {
                        number = 0,
                        type = isBlock ? "block" : "standard"
                    };

                    newBoard.cells.Add(newCell);
                }
            }

            // Fill Numbers

            JSONBoard previousJsonBoard = newBoard;
            SolvingResult solvingResult = MainSolver.SolveBoard(previousJsonBoard);


            while (true)
            {
                newBoard = MakeJSONBoardCopy(previousJsonBoard);

                /// fülle eine neue zufällig gewählte Zahl in das Board ein
                var new_cell_number = -1;

                while(new_cell_number == -1)
                {
                    // finde random Cell die noch nicht gefüllt ist. Dies wird die neue FillCell
                    var potential_new_cell_number = random.Next(newBoard.cells.Count);                
                    if (newBoard.cells[potential_new_cell_number].number == 0 
                        && solvingResult.UnsolvedBoard.Cells.Find(x => x.index == potential_new_cell_number).value == 0) // Manchmal ergeben sich die Values bereits logisch im SolvingErgebnis. Solche Zellen brauchen nicht gesetzt werden
                        new_cell_number = potential_new_cell_number;
                }

                var cell_to_fill = newBoard.cells[new_cell_number];
                var cell_is_block = cell_to_fill.type == "block";

                if (!cell_is_block)
                {
                    // Bei Non Blocks wähle zufällig eine der möglichen Numbers aus
                    var possible_numbers = solvingResult.UnsolvedBoard.Cells.Find( x => x.index == new_cell_number).possibleValues;
                    var chosen_number = possible_numbers[random.Next(possible_numbers.Count)];
                    cell_to_fill.number = chosen_number;
                }
                else if (cell_is_block)
                {
                    // Bei Blocks finde raus welche Zahlen in Col/Row Partnerzellen bereits belegt sind. Diese kommen nicht in Frage
                    var possible_numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                    var cell_row_pos = Convert.ToInt32(Math.Floor(new_cell_number / 9f));
                    var cell_col_pos = new_cell_number % 9;

                    var partnerZellen = solvingResult.UnsolvedBoard.Cells.Where(cell =>                   
                        // Eine Partnerzelle liegt in selber Row Oder Column
                        cell.row_pos == cell_row_pos || cell.col_pos == cell_col_pos
                    ).ToList();

                    partnerZellen.ForEach(x => possible_numbers.Remove(x.value));

                    if (possible_numbers.Count == 0)
                    {
                        // Sind keine übrig so kann dieser Block keine Number halten
                        continue;
                    }

                    cell_to_fill.number = possible_numbers[random.Next(possible_numbers.Count)];
                }

                /// Nachdem die Zahl eingefügt wurde, solve das Board und werte das Ergebnis aus

                var copy_solving_result = MainSolver.SolveBoard(newBoard);

                if (copy_solving_result.ResultType == ResultType.Success)
                {
                    // Board is finished and fully generated
                    solvingResult = copy_solving_result;
                    break; 
                }
                    

                if (copy_solving_result.ResultType == ResultType.NoSolution)
                {
                    // Im Falle einer NoSolution sollte vorheriger BoardState wiederhergestellt werden,
                    // die letzte Änderung also Rückgängig gemacht werden
                    continue;
                }

                if (copy_solving_result.ResultType == ResultType.MultipleSolutions)
                {
                    // Die Änderung war gültig das Board ist aber noch nicht fertig
                    previousJsonBoard = newBoard;
                    solvingResult = copy_solving_result;
                    continue;
                }
            }
            
            return newBoard;
        }

        private static JSONBoard MakeJSONBoardCopy(JSONBoard original)
        {
            var copy = new JSONBoard
            {
                size = original.size
            };

            foreach (var cell in original.cells)
            {
                copy.cells.Add(new JSONBoardCell
                {
                    number = cell.number,
                    type = cell.type
                });
            }

            return copy;
        }
    }
}