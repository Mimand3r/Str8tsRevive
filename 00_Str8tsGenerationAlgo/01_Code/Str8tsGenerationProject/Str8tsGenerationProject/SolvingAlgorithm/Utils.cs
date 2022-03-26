using Newtonsoft.Json;
using Str8tsGenerationProject.JSON;
using Str8tsGenerationProject.SolvingAlgorithm.Exceptions;
using Str8tsGenerationProject.SolvingAlgorithm.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Str8tsGenerationProject.SolvingAlgorithm
{
    internal static class Utils
    {
        internal static bool IntListsAreEqual(this List<int> first_list, List<int> second_list)
        {
            if (first_list.Count != second_list.Count) return false;

            foreach (var first_list_entry in first_list)
            {
                if (!second_list.Contains(first_list_entry)) return false;
            }

            return true;
        }

        //internal static List<dynamic> CreateCellOptionsJson(this SolverBoard board)
        //{
        //    var cell_list = new List<dynamic>();

        //    foreach (var cell in board.Cells)
        //    {
        //        var cell_dictionary = new Dictionary<string, dynamic>();

        //        cell_dictionary.Add("is_solved", cell.isSolved);
        //        cell_dictionary.Add("is_block", cell.isBlock);
        //        cell_dictionary.Add("index", cell.index);
        //        cell_dictionary.Add("row", cell.row_pos);
        //        cell_dictionary.Add("col", cell.col_pos);

        //        if (!cell.isSolved)
        //        {
        //            cell_dictionary.Add("value", "");
        //            cell_dictionary.Add("possibilities", cell.possibleValues);
        //        }

        //        else
        //        {
        //            cell_dictionary.Add("value", cell.value);
        //            cell_dictionary.Add("possibilities", new List<int>());
        //        }

        //        cell_list.Add(cell_dictionary);
        //    }

        //    return cell_list;
        //}

        internal static JSONBoard ConvertToJSONBoard(this SolverBoard board)
        {
            var output = new JSONBoard
            {
                size = board.size,
                cells = board.Cells.Select(x=> new JSONBoardCell
                {
                    type = x.isBlock? "block" : "standard",
                    number = x.value
                }).ToList()
            };

            return output;
        }

        //internal static void WriteToJsonFile(dynamic json_object)
        //{
        //    string json = JsonConvert.SerializeObject(json_object);
        //    // Show Save File Dialog
        //    var saveFileDialog = new SaveFileDialog();

        //    saveFileDialog.Filter = "JSON files (*.json)|*.json;";
        //    saveFileDialog.FilterIndex = 0;
        //    saveFileDialog.RestoreDirectory = true;

        //    if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

        //    var filename = saveFileDialog.FileName;

        //    File.WriteAllText(filename, json);
        //}

        internal static void chooseNextOption(SolverBoard solver_board_copy, out int cell_index, out int filled_value, int counter)
        {
            var unsolved_cells = solver_board_copy.Cells.Where(x => !x.isSolved).ToList();
            unsolved_cells.Sort((x, y) => x.possibleValues.Count - y.possibleValues.Count);

            SolverCell chosen_cell = null;
            foreach (var unsolved_cell in unsolved_cells)
            {
                if (counter >= unsolved_cell.possibleValues.Count)
                {
                    counter -= unsolved_cell.possibleValues.Count;
                    continue;
                }

                chosen_cell = unsolved_cell;
                break;
            }

            if (chosen_cell == null)
                throw new MultipleSolutionsException();

            var chosen_option = chosen_cell.possibleValues[counter];

            chosen_cell.value = chosen_option;
            chosen_cell.isSolved = true;
            chosen_cell.possibleValues.Clear();

            cell_index = chosen_cell.index;
            filled_value = chosen_option;

            solver_board_copy.currently_testing_cell_index = cell_index;
        }
    }
}
