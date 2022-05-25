using Str8tsGenerationProject.JSON;
using Str8tsGenerationProject.SolvingAlgorithm.Exceptions;
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
    }
}
