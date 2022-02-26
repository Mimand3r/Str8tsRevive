﻿using Str8tsGenerationProject.JSON; 
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
                /// In dem Solverboard Constructor werden alle Cells aus dem jsonBoard extrahiert und es werden SolverCells erstellt. 
                /// Jede Solver Cell enthällt Koordinateninformation (index, row-pos, col-pos), typ information (isBlock, value), state information isSolved und eine possibility Liste
                /// Die Cell Possibility Liste ist eine rechenintensive Größe die von Row/Col Kollegenzellen und Strate Kollegenzellen abhängt. 
                /// Am Anfang wird die Cell Possibility Liste mit Werten von 1-9 gefüllt was allen Optionen entspricht
                var solver_board = SolverBoard.createEmptyBoard(jsonBoard);

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


                /// Solving Loop 
                while (solver_board.isUnsolved)
                {

                    try
                    {
                        /// Zuerst werden die Str8te Possibilities neu calculated. Herangezogen werden hierfür eventuelle neue Entrys in den SolvingLists
                        /// hierbei können sich neue Entrys in must include ergeben die dann CannotInclude Entrys in sister str8tes erzeugen
                        /// wann immer dies passiert ist, so muss die Possibility Calculation neu gestartet werden
                        solver_board.RecalculateAllStr8tePossibilities();

                        /// Als nächstes werden die Cell Possible-Values neu kalkuliert/reduziert
                        /// zuerst werden alle Possible-Values gestrichen, die in Row oder Col bereits benutzt werden
                        /// dann werden alle Possible-Values gestrichen, die in CannotInclude von einem der beiden Cell-Str8tes auftauchen oder außerhalb aller Range options für eine der Str8tes liegen
                        solver_board.RecalculateAllCellPossibilities();

                    }
                    catch (NoSolutionException e)
                    {
                        /// Bei den Recalculate Funktionen kann es passieren dass die Options für eine Str8te oder Cell auf 0 fallen.
                        /// Ist dies der Fall so ist das Board nicht lösbar
                        throw e;
                    }

                    /// Manchmal gibt es must-includes in Str8tes die nur eine der Zellen erfüllen kann (Hero Zellen).
                    /// Für eine solche Hero Zelle kollabieren alle anderen Optionen und sie erfüllt ihre Hero Rolle
                    /// Dieses Szenario wird hier abgehandelt und reduziert die Cell Options in dem Fall auf 1 Element
                    //solver_board.CollapseOptionsForMustIncludeHeroCells();

                    /// Das Solving Step Possibility Matrix sollte an dieser Stelle ins Solving Log aufgenommen werden

                    /// Nun gibt es ein Solving Step Ergebnis.
                    /// Dieses besteht aus der Possibility List für Alle Zellen
                    /// gibt es keine Zellen mit nur einer Possibility so gibt es kein uniques Ergebnis für das Board              
                    if (solver_board.Cells.All( x => x.possibleValues.Count > 1))
                    {
                        throw new MultipleSolutionsException();
                    }

                    /// Nun kann eine beliebige Zelle der Länge 1 gewählt werden und die Possibilty als Value gesetzt werden. Dadurch gilt die Zelle als solved
                    /// Weiterhin müssen die Str8tes der Zelle geupdated werden. Sie bekommen den neuen Eintrag als AlreadyInclude Entrys und Sister Str8tes bekommen entsprechend CannotInclude Einträge
                    /// Nun muss gechecked werden ob das Board als solved gilt

                    /// Die gewählte Zahl sollte an dieser Stelle ins Solving Log aufgenommen werden
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
