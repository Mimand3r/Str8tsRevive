using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Str8tsGenerationProject.SolvingAlgorithm
{
    public static class Utils
    {
        public static bool IntListsAreEqual(this List<int> first_list, List<int> second_list)
        {
            if (first_list.Count != second_list.Count) return false;

            foreach (var first_list_entry in first_list)
            {
                if (!second_list.Contains(first_list_entry)) return false;
            }

            return true;
        }
    }
}
