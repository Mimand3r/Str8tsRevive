using Newtonsoft.Json;
using Str8tsGenerationProject.SolvingAlgorithm.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        internal static List<dynamic> CreateCellOptionsJson(this SolverBoard board)
        {
            var cell_list = new List<dynamic>();

            foreach (var cell in board.Cells)
            {
                var cell_dictionary = new Dictionary<string, dynamic>();

                cell_dictionary.Add("is_solved", cell.isSolved);
                cell_dictionary.Add("is_block", cell.isBlock);
                cell_dictionary.Add("index", cell.index);
                cell_dictionary.Add("row", cell.row_pos);
                cell_dictionary.Add("col", cell.col_pos);

                if (!cell.isSolved)
                {
                    cell_dictionary.Add("value", "");
                    cell_dictionary.Add("possibilities", cell.possibleValues);
                }

                else
                {
                    cell_dictionary.Add("value", cell.value);
                    cell_dictionary.Add("possibilities", new List<int>());
                }

                cell_list.Add(cell_dictionary);
            }

            return cell_list;
        }

        internal static void WriteToJsonFile(dynamic json_object)
        {
            string json = JsonConvert.SerializeObject(json_object);
            File.WriteAllText(@"D:\Heinrich\Projekte\007_Str8tsRevive\00_Str8tsGenerationAlgo\01_Code\Str8tsGenerationProject\Str8tsGenerationProject\JSON\solving_steps_dumps\test.json", json);
        }

    }
}
