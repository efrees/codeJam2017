using System.Linq;
using CodeJamQualification2017;
using NUnit.Framework;

namespace QualificationTests
{
    internal class ProgramD_should_
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
            var score = ProgramD.ScoreGrid(grid);
            Assert.AreEqual(5, score);
        }

        [Test]
        public void make_no_changes_if_grid_is_optimal()
        {
            var grid = new[,] { { 'o' } };
            var changes = ProgramD.GetChangesForBestScore(grid);
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

            Assert.IsTrue(ProgramD.DiagonalNotUsed(grid, new GridCoordinates {Row = 1, Col = 0}));
            Assert.IsTrue(ProgramD.DiagonalNotUsed(grid, new GridCoordinates {Row = 0, Col = 0}));
            Assert.IsFalse(ProgramD.DiagonalNotUsed(grid, new GridCoordinates {Row = 0, Col = 1}));
        }
        
        [Test]
        public void add_models_when_it_improves_score()
        {
            var grid = new[,]
            {
                { '.', 'x' },
                { '.', '.' }
            };
            var changes = ProgramD.GetChangesForBestScore(grid);
            CollectionAssert.AreEquivalent(new []{ "o 2 1", "+ 1 1" }, changes);
        }

        [Test]
        public void overwrite_existing_when_necessary()
        {
            var grid = new[,]
            {
                {'.', 'x', '.'},
                {'+', '+', '+'},
                {'x', '.', '.'}
            };

            var changes = ProgramD.GetChangesForBestScoreBruteForce(grid, 3);
            CollectionAssert.AreEquivalent(new[] { "o 2 3" }, changes.Select(c => c.ToString()));
        }
    }
}
