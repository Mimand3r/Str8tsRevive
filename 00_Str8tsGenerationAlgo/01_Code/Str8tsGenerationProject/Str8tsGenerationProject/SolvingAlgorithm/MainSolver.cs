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
    public class MainSolver
    {
        public static SolvingResult SolveBoard(JSONBoard jsonBoard,  Boolean isInSimulationMode = false, SolverBoard solver_board = null)
        {
            try
            {
                /// In dem Solverboard Constructor werden alle Cells aus dem jsonBoard extrahiert und es werden SolverCells erstellt. 
                /// Jede Solver Cell enthällt Koordinateninformation (index, row-pos, col-pos), typ information (isBlock, value), state information isSolved und eine possibility Liste
                /// Die Cell Possibility Liste ist eine rechenintensive Größe die von Row/Col Kollegenzellen und Strate Kollegenzellen abhängt. 
                /// Am Anfang wird die Cell Possibility Liste mit Werten von 1-9 gefüllt was allen Optionen entspricht
                if (solver_board == null) 
                {
                    solver_board = SolverBoard.createEmptyBoard(jsonBoard);
                }

                /// Neben einer Menge an Zellen besteht ein Board aus einer Menge an Str8tes. Jede non blocktype Cell ist Teil von 2 Str8tes einer vertikalen und einer horizontalen
                /// Eine Str8te besteht aus Koordinateninformation (index, row_start, col_start, length), es gibt 2 Arten von Str8tes horizontal/vertical
                /// Sie besteht weiterhin aus einer Menge an Cells die zu einer Str8te gegrouped werden
                /// Weiterhin hällt eine Str8te NumberArrays die sich im Laufe des Solving prozesses verändern und am Ende eines SolvingSteps werden diese benutzt um die Cell Options anzupassen die dann den Output eines Solving Steps bilden
                /// 1. mustInclude -> diese Werte müssen von der Str8te zwingend eingebunden werden und bilden CannotInclude entrys für sister str8tes. Neue entrys ergeben sich hier wenn sie in allen Possibility optionen der Str8te auftauchen
                /// 2. AlreadyIncludes -> Werte die eine der Str8te Cells bereits eingebunden hat. Bilden immer CannotInclude für sister Str8tes
                /// 3. CannotInclude -> Werte die für die Str8te verboten sind. Entrys erscheinen hier vor allem durch sister str8tes (und am Anfang durch sister blocks)
                /// 
                /// Weiterhin gibt es eine Possibilities List. Sie repräsentiert alle Ranges die die Str8te annehmen kann. Beim Updaten der Possibilities werden die SolvingArrays benutzt
                /// 
                /// Initial werden alle Str8tes erstellt und die Koordinaten und Typ Information gefüllt
                /// die AlreadyIncludes values werden gefüllt gemäß den StartValues beteiligter Cells und erzeugen Cannot Includes bei sister Str8tes
                /// Cannot Includes werden auch gefüllt wenn sister Block Values bei Str8tes auftauchen
                /// 
                /// Die Possibility Liste wird maximal gefüllt und ignoriert dabei im initialen Schritt die NumberArrays
                solver_board.CreateCleanStraightsLayout();


                var retryCounter = 0;
                /// Solving Loop 
                while (solver_board.isUnsolved)
                {
                        /// Zuerst werden die Str8te Possibilities neu calculated. Herangezogen werden hierfür eventuelle neue Entrys in den SolvingLists
                        /// hierbei können sich neue Entrys in must include ergeben die dann CannotInclude Entrys in sister str8tes erzeugen
                        /// wann immer dies passiert ist, so muss die Possibility Calculation neu gestartet werden
                        solver_board.RecalculateAllStr8tePossibilities();

                        /// Als nächstes werden die Cell Possible-Values neu kalkuliert/reduziert
                        /// zuerst werden alle Possible-Values gestrichen, die in Row oder Col bereits benutzt werden (nicht nötig wenn cannot includes der beiden str8tes ausgewertet werden)
                        /// dann werden alle Possible-Values gestrichen, die in CannotInclude von einem der beiden Cell-Str8tes auftauchen, die in Already Includes auftauchen oder außerhalb aller Range options für eine der Str8tes liegen
                        solver_board.RecalculateAllCellPossibilities();

                        /// Manchmal gibt es aufgrund von cross-dependencys zwischen hor/vert str8tes situationen in denen str8te options aufgrund der neuen Zellensituation nicht mehr abgebildet werden können
                        /// diese Str8te options müssen hier rausgefiltered werden. Das erfolgt indem der Str8te range-is-forbidden entrys hinzugefügt werden
                        /// wann immer dies passiert, muss der solving step restartet werden
                        var anyStr8teGotNewForbiddenEntrys = solver_board.ResolveCrossDependencies();
                        if (anyStr8teGotNewForbiddenEntrys)
                            continue; 

                        /// Manchmal gibt es row-wise oder col-wise Paare
                        /// Paare sind dependent cells deren optionen gleich sind. Ist die Anzahl der Paar Elemente gleich der geteilten Optionslänge so bilden diese Elemente ein Paar
                        /// Paare entfernen ihre Optionen bei allen anderen Cell Elementen in der gesamten Row/Col Str8te übergreifend
                        /// Hierbei können neue cannot include entrys & must include entrys für die beteiligten Str8tes entstehen. Wann immer das passiert, muss der solving step neu gestartet werden
                        var anyStr8teGotNewEntrys = solver_board.CollapseCellOptionsIfPaarelementeExist();
                        if (anyStr8teGotNewEntrys)
                            continue; /// paare können cells aber auch str8tes (mustinclude/cannotinclude) verändern. Gab es eine str8te-Veränderung, so müssen die possibilities neu calculated werden. daher erfolgt ein solving step restart

                        /// Manchmal gibt es must-includes in Str8tes die nur eine der Zellen erfüllen kann (Hero Zellen).
                        /// Für eine solche Hero Zelle kollabieren alle anderen Optionen und sie erfüllt ihre Hero Rolle
                        /// Dieses Szenario wird hier abgehandelt und reduziert die Cell Options im Falle einer HeroCell auf 1 Element
                        var made_changes = solver_board.CollapseCellOptionsForMustIncludeHeroCells();
                        if (made_changes)
                            continue;

                        /// Nun können Zellen mit nur einer possibility entstanden sein. wenn dem so ist so müssen diese gefülled werden uns als isSolved markiert werden
                        /// die beiden str8ts der zelle bekommen nun einen already include entry und sister str8tes einen cannot include entry
                        /// wann immer hier modifikationen stattfanden so wird der solving step restarted

                        var edited = solver_board.FillSolvedCells();
                        if (edited && !solver_board.isUnsolved)
                            continue;

                    /// Simulation Mode
                    /// gibt es immer noch unfilled Cells? 
                    /// mache eine Kopie des SolvingBoards und führe die selbe Funktion im simulation modus aus. das Ziel ist es einen Wiederspruch zu entdecken und die gewählte Number ausschließen zu können

                    if (isInSimulationMode) return null;

                        if (solver_board.isUnsolved)
                        {
                            /// Take Cell with least options and fill 1 randomly
                            /// now start sumilation run and hope for a no solution exception
                            /// if it accured remove that option from the cell and rerun the solving step
                            /// if it didnt accure try again chossing the next value

                            var removed_an_option = false;
                            var counter = 0;
                            while (true)
                            {
                                SolverBoard solver_board_copy = solver_board.MakeDeepCopy();

                                var optionsArray = solver_board_copy.createOptionsArray();

                                if (counter >= optionsArray.Count)
                                {
                                    /// in diesem Fall konnte keine Option ausgeschlossen werden.
                                    /// Das Board enthällt keine eindeutige Lösung mehr da alle remaining Optionen zu einer von meheren Solutions führen

                                    throw new MultipleSolutionsException();
                                }

                                var optionToTest = optionsArray[counter];

                                /// Es wird eine Option 'ausprobiert'. Ziel ist es diese ausschließen zu können um sie aus der Optionsliste ausschließen zu können
                                solver_board_copy.fillOption(optionToTest);
                                
                                try
                                {
                                    SolveBoard(null, true, solver_board_copy);

                                    /// Hat der Algorithmus zufällig eine korrekte Zahl gewählt, so kann es sein, dass das Board nun völlig gelöst wurde
                                    /// Trotzdem kann diese Lösung nicht benutzt werden da sie eine von mehreren Lösungen sein kann.
                                    /// Wir wollen aber sicher gehen dass das Board nur eine gültige Lösung hat, daher können nur Optionen benutzt werden die eine Zahl ausschließen
                                    /// konnte nichts ausgeschlossen werden dann muss der counter hochgezählt werden. dieser stellt sicher, dass nicht die selbe option gewählt wird

                                    counter += 1;
                                    continue;
                                }
                                catch (NoSolutionException e)
                                {
                                    var solver_cell = solver_board.Cells.Find(x => x.index == optionToTest.Item1);
                                    solver_cell.possibleValues.Remove(optionToTest.Item2);
                                    if (solver_cell.possibleValues.Count == 1)
                                    {
                                        solver_board.FillSolvedCells();
                                    }
                                    removed_an_option = true;
                                    break;
                                }
                            }

                            if (removed_an_option)
                                continue;

                        }
                        
                }

                /// Diese Funktion erzeugt den Solving Step Output. Es ist eine Matrix die für alle Unsolved Cells die Optionen hällt
                //var cell_options_json = solver_board.CreateCellOptionsJson();
                //Utils.WriteToJsonFile(cell_options_json);
            }
            catch (NoSolutionException e)
            {
                /// Wenn eine simmulierte funktion diese Exception auslöst
                if (!solver_board.isOriginal)
                    throw e;

                return new SolvingResult
                {
                    ResultType = ResultType.NoSolution
                };
            }
            catch (MultipleSolutionsException e)
            {
                return new SolvingResult
                {
                    ResultType = ResultType.MultipleSolutions
                };
            }

            // Convert to JSONBoard
            var solvedJsonBoard = solver_board.ConvertToJSONBoard();

            return new SolvingResult
            {
                ResultType = ResultType.Success,
                SolvedBoard = solvedJsonBoard
            };

        }
    }
}
