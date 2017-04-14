using CodeJamQualification2017;
using NUnit.Framework;

namespace QualificationTests
{
    internal class ProgramDPostContest_should_
    {
        [Test]
        public void score_grid_correctly()
        {
            var grid = new[,]
            {
                { '.', 's', 'x'},
                { '+', '.', 'o'},
                { 'x', 'y', 'z'}
            };
            var score = ProgramDPostContest.ScoreGrid(grid);
            Assert.AreEqual(5, score);
        }

        [Test]
        public void make_no_changes_if_grid_is_optimal()
        {
            var grid = new[,] { { 'o' } };
            var optimalSolution = ProgramDPostContest.GetOptimalSolution(grid);
            var changes = ProgramDPostContest.GetChangesFromInitial(grid, optimalSolution);
            CollectionAssert.IsEmpty(changes);
        }

        [Test]
        public void check_diagonal_correctly()
        {
            var grid = new[,]
            {
                {'.', 'x', '.'},
                {'.', '.', '+'},
                {'o', '.', '.'}
            };

            Assert.IsTrue(ProgramDPostContest.DiagonalNotUsed(grid, new GridCoordinates { Row = 1, Col = 0 }));
            Assert.IsTrue(ProgramDPostContest.DiagonalNotUsed(grid, new GridCoordinates { Row = 0, Col = 0 }));
            Assert.IsFalse(ProgramDPostContest.DiagonalNotUsed(grid, new GridCoordinates { Row = 0, Col = 1 }));
        }

        [Test]
        public void add_models_when_it_improves_score()
        {
            var grid = new[,]
            {
                { '.', 'x' },
                { '.', '.' }
            };
            var solution = ProgramDPostContest.GetOptimalSolution(grid);
            var changes = ProgramDPostContest.GetChangesFromInitial(grid, solution);
            CollectionAssert.AreEquivalent(new[] { "+ 1 1", "o 1 2", "x 2 1" }, changes);
        }
        
        [Test]
        public void solve_rook_subproblem()
        {
            var initialGrid = new[,]
            {
                {'.', 'x', '.'},
                {'+', '+', '+'},
                {'x', '.', '.'}
            };

            var expectedGrid = new[,]
            {
                {'.', 'x', '.'},
                {'.', '.', 'x'},
                {'x', '.', '.'}
            };

            var rookSolution = ProgramDPostContest.SolveRookSubProblem(initialGrid);

            CollectionAssert.AreEqual(expectedGrid, rookSolution);
        }

        [Test]
        public void solve_bishop_subproblem()
        {
            var initialGrid = new[,]
            {
                {'.', 'x', '.'},
                {'+', '.', '+'},
                {'x', '.', '.'}
            };

            var expectedGrid = new[,]
            {
                {'+', '.', '+'},
                {'+', '.', '+'},
                {'.', '.', '.'}
            };

            var bishopSolution = ProgramDPostContest.SolveBishopSubProblem(initialGrid);

            CollectionAssert.AreEqual(expectedGrid, bishopSolution);
        }

        [Test]
        public void combine_subproblems_correctly()
        {
            var rookGrid = new[,]
            {
                {'.', 'x', '.'},
                {'.', '.', 'x'},
                {'x', '.', '.'}
            };

            var bishopGrid = new[,]
            {
                {'.', '.', '+'},
                {'+', '.', '+'},
                {'.', '.', '.'}
            };

            var expectedGrid = new[,]
            {
                {'.', 'x', '+'},
                {'+', '.', 'o'},
                {'x', '.', '.'}
            };

            var actualSolution = ProgramDPostContest.MergeSubProblemSolutions(rookGrid, bishopGrid);
            CollectionAssert.AreEqual(expectedGrid, actualSolution);
        }

        [Test]
        public void list_differences_from_initial()
        {
            var initialGrid = new[,]
            {
                {'.', 'x', '.'},
                {'+', '.', '+'},
                {'x', '.', '.'}
            };

            var finalGrid = new[,]
            {
                {'.', 'x', '+'},
                {'+', '.', 'o'},
                {'x', '.', '.'}
            };

            var expectedChanges = new[]
            {
                "+ 1 3",
                "o 2 3"
            };

            var actual = ProgramDPostContest.GetChangesFromInitial(initialGrid, finalGrid);
            CollectionAssert.AreEquivalent(expectedChanges, actual);
        }
    }
}
