using Str8tsGenerationProject.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Str8tsGenerator.Types
{
    public class GenerationResult
    {
        public JSONBoard EmptyBoard { get; set; }
        public JSONBoard Solution { get; set; }
    }
}
