using Microsoft.VisualStudio.TestTools.UnitTesting;
using Str8tsGenerationProject.SolvingAlgorithm.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Str8tsGenerationProject.SolvingAlgorithm.Types.Tests
{
    [TestClass()]
    public class SolverBoardTests
    {
        [TestMethod()]

        // Test S1,B,S1,B,S1 -> die 3 straights können alle Werte zwischen 1-5 annehmen
        public void Test001_Basic()
        {
            var straights = new List<Str8te> {
                new Str8te
                {
                    index = 0,
                    length = 1,
                    Cells = new List<SolverCell>
                    {
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 0, 5),
                    },
                },
                new Str8te
                {
                    index = 2,
                    length = 1,
                    Cells = new List<SolverCell>
                    {
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 0, 5),
                    },
                },
                new Str8te
                {
                    index = 4,
                    length = 1,
                    Cells = new List<SolverCell>
                    {
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 0, 5),
                    },
                }
            };


            SolverBoard.calcAllStratePossibilitiesForRowCol(straights, 2, 5);

            // Possibilities must now be set to 1-5 for every entry
            foreach (var straight in straights)
            {
                Assert.IsTrue(straight.Possibilities.Any(x => x.isInRange(1)));
                Assert.IsTrue(straight.Possibilities.Any(x => x.isInRange(2)));
                Assert.IsTrue(straight.Possibilities.Any(x => x.isInRange(3)));
                Assert.IsTrue(straight.Possibilities.Any(x => x.isInRange(4)));
                Assert.IsTrue(straight.Possibilities.Any(x => x.isInRange(5)));
                Assert.IsTrue(straight.Possibilities.Count == 5);
                Assert.IsTrue(straight.MustInclude.Count == 0);
            }
        }

        [TestMethod()]

        // Test S5,S5,S5,S5,S5 -> Eine Strate die fixe Range 1-5 hat und alle Zahlen sind als Must Include markiert
        public void Test002_VolleStrate()
        {
            var straights = new List<Str8te> {
                new Str8te
                {
                    index = 0,
                    length = 5,
                    Cells = new List<SolverCell>
                    {
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 0, 5),
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 1, 5),
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 2, 5),
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 3, 5),
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 4, 5),
                    },
                },
            };


            SolverBoard.calcAllStratePossibilitiesForRowCol(straights, 0, 5);

            var straight = straights[0];

            // Darf nur eine Possiblity 1 - 5 geben
            Assert.IsTrue(straight.Possibilities.Count == 1);
            Assert.IsTrue(straight.Possibilities[0].isInRange(1));
            Assert.IsTrue(straight.Possibilities[0].isInRange(2));
            Assert.IsTrue(straight.Possibilities[0].isInRange(3));
            Assert.IsTrue(straight.Possibilities[0].isInRange(4));
            Assert.IsTrue(straight.Possibilities[0].isInRange(5));

            // Must Include muss alle Elemente von 1 - 5 enthalten
            Assert.IsTrue(straight.MustInclude.Count == 5);
            Assert.IsTrue(straight.MustInclude.Contains(1));
            Assert.IsTrue(straight.MustInclude.Contains(2));
            Assert.IsTrue(straight.MustInclude.Contains(3));
            Assert.IsTrue(straight.MustInclude.Contains(4));
            Assert.IsTrue(straight.MustInclude.Contains(5));
        }

        [TestMethod()]
        // Test S1,B,S3,S3,S3
        public void Test003_3erUnd1erStrate()
        {
            var straights = new List<Str8te> {
                new Str8te
                {
                    index = 0,
                    length = 1,
                    Cells = new List<SolverCell>
                    {
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 0, 5),
                    },
                },
                new Str8te
                {
                    index = 2,
                    length = 3,
                    Cells = new List<SolverCell>
                    {
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 2, 5),
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 3, 5),
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 4, 5),
                    },
                },
            };


            SolverBoard.calcAllStratePossibilitiesForRowCol(straights, 1, 5);

            var straight1 = straights[0];
            var straight2 = straights[1];

            // straight1 darf alle values 1-5 annehmen außer 3!, da diese required in anderer strate ist
            Assert.IsTrue(straight1.Possibilities.Any(x => x.isInRange(1)));
            Assert.IsTrue(straight1.Possibilities.Any(x => x.isInRange(2)));
            Assert.IsTrue(straight1.Possibilities.Any(x => x.isInRange(4)));
            Assert.IsTrue(straight1.Possibilities.Any(x => x.isInRange(5)));
            Assert.IsTrue(straight1.Possibilities.Count == 4);
            Assert.IsTrue(straight1.MustInclude.Count == 0);

            // straight2 muss 3 erlaubte Ranges haben 1-3 2-4 3-5 -> mit must include = 3

            Assert.IsTrue(straight2.Possibilities.Count == 3);
            Assert.IsTrue(straight2.Possibilities[0].isFromTo(1, 3));
            Assert.IsTrue(straight2.Possibilities[1].isFromTo(2, 4));
            Assert.IsTrue(straight2.Possibilities[2].isFromTo(3, 5));
            Assert.IsTrue(straight2.MustInclude.Count == 1);
            Assert.IsTrue(straight2.MustInclude.Contains(3));

        }

        [TestMethod()]
        // Test S1,B,S3=4,S3,S3
        public void Test004_3erUnd1erStrate_a()
        {
            var straights = new List<Str8te> {
                new Str8te
                {
                    index = 0,
                    length = 1,
                    Cells = new List<SolverCell>
                    {
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 0, 5),
                    },
                    CannotInclude = new List<int>{4}
                },
                new Str8te
                {
                    index = 2,
                    length = 3,
                    Cells = new List<SolverCell>
                    {
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 4 }, index: 2, 5),
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 3, 5),
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 4, 5),
                    },
                    AlreadyIncludes = new List<int>{4}
                },
            };


            SolverBoard.calcAllStratePossibilitiesForRowCol(straights, 1, 5);

            var straight1 = straights[0];
            var straight2 = straights[1];

            // straight1 darf folgende values (range l=1) haben: 1, 2, 5
            Assert.IsTrue(straight1.Possibilities.Any(x => x.isInRange(1)));
            Assert.IsTrue(straight1.Possibilities.Any(x => x.isInRange(2)));
            Assert.IsTrue(straight1.Possibilities.Any(x => x.isInRange(5)));
            Assert.IsTrue(straight1.Possibilities.Count == 3);
            Assert.IsTrue(straight1.MustInclude.Count == 0);

            // straight2 hat die ranges: 3-5, 2-4 & must include = 3
            Assert.IsTrue(straight2.Possibilities.Count == 2);
            Assert.IsTrue(straight2.Possibilities.Any(x => x.isFromTo(3, 5)));
            Assert.IsTrue(straight2.Possibilities.Any(x => x.isFromTo(2, 4)));
            Assert.IsTrue(straight2.MustInclude.Count == 1);
            Assert.IsTrue(straight2.MustInclude.Contains(3));

        }

        [TestMethod()]
        // Test S1,B=4,S3,S3,S3
        public void Test005_3erUnd1erStrate_b()
        {
            var straights = new List<Str8te> {
                new Str8te
                {
                    index = 0,
                    length = 1,
                    Cells = new List<SolverCell>
                    {
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 0, 5),
                    },
                    CannotInclude = new List<int>{4}
                },
                new Str8te
                {
                    index = 2,
                    length = 3,
                    Cells = new List<SolverCell>
                    {
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 2, 5),
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 3, 5),
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 4, 5),
                    },
                    CannotInclude = new List<int>{4}
                },
            };


            SolverBoard.calcAllStratePossibilitiesForRowCol(straights, 1, 5);

            var straight1 = straights[0];
            var straight2 = straights[1];

            // straight1 darf folgende values (range l=1) haben: 5
            Assert.IsTrue(straight1.Possibilities.Any(x => x.isInRange(5)));
            Assert.IsTrue(straight1.Possibilities.Count == 1);
            Assert.IsTrue(straight1.MustInclude.Count == 1);
            Assert.IsTrue(straight1.MustInclude[0] == 5);

            // straight2 hat die ranges: 1-3 und must include = 1,2,3
            Assert.IsTrue(straight2.Possibilities.Count == 1);
            Assert.IsTrue(straight2.Possibilities.Any(x => x.isFromTo(1, 3)));
            Assert.IsTrue(straight2.MustInclude.Count == 3);
            Assert.IsTrue(straight2.MustInclude.Contains(3));
            Assert.IsTrue(straight2.MustInclude.Contains(1));
            Assert.IsTrue(straight2.MustInclude.Contains(2));

        }

        [TestMethod()]
        // Test S1=4,B,S3,S3,S3
        public void Test006_3erUnd1erStrate_c()
        {
            var straights = new List<Str8te> {
                new Str8te
                {
                    index = 0,
                    length = 1,
                    Cells = new List<SolverCell>
                    {
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 4 }, index: 0, 5),
                    },
                    AlreadyIncludes = new List<int>{4}
                },
                new Str8te
                {
                    index = 2,
                    length = 3,
                    Cells = new List<SolverCell>
                    {
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 2, 5),
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 3, 5),
                        new SolverCell(new JSON.JSONBoardCell{ type = "standard", number = 0 }, index: 4, 5),
                    },
                    CannotInclude = new List<int>{4}
                },
            };


            SolverBoard.calcAllStratePossibilitiesForRowCol(straights, 1, 5);

            var straight1 = straights[0];
            var straight2 = straights[1];

            // straight1 ist bereits gesetzt = 4
            Assert.IsTrue(straight1.Possibilities.Count == 1);
            Assert.IsTrue(straight1.Possibilities.Any(x => x.isFromTo(4, 4)));
            Assert.IsTrue(straight1.MustInclude.Count == 0);
            Assert.IsTrue(straight1.AlreadyIncludes.Count == 1);
            Assert.IsTrue(straight1.AlreadyIncludes.Contains(4));
            Assert.IsTrue(straight1.CannotInclude.Count == 3);
            Assert.IsTrue(straight1.CannotInclude.Contains(1));
            Assert.IsTrue(straight1.CannotInclude.Contains(2));
            Assert.IsTrue(straight1.CannotInclude.Contains(3));

            // straight2 hat die ranges: 1-3 und must include = 1,2,3
            Assert.IsTrue(straight2.Possibilities.Count == 1);
            Assert.IsTrue(straight2.Possibilities.Any(x => x.isFromTo(1, 3)));
            Assert.IsTrue(straight2.MustInclude.Count == 3);
            Assert.IsTrue(straight2.MustInclude.Contains(3));
            Assert.IsTrue(straight2.MustInclude.Contains(1));
            Assert.IsTrue(straight2.MustInclude.Contains(2));

        }

    }
}