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
    public class MainSolver
    {
        public static SolvingResult SolveBoard(JSONBoard jsonBoard)
        {
            try
            {
                // Convert to Solver-Types
                var solver_board = new SolverBoard(jsonBoard);
                solver_board.CreateAllOptionsForAllUnfilledFields(); // Fills all Options with 1-9

                // start solving
                while (solver_board.isUnsolved)
                {
                    solver_board.RemoveOptionsAccordingToRowCol();
                    solver_board.CreateCleanStraightsLayout();
                }


                return null;
            }
            catch (MultipleSolutionsException)
            {
                // TODO handle
                throw;
            }
            catch (NoSolutionException)
            {
                // TODO handle
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
