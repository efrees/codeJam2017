using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeJamQualification2017
{
    class ProgramD
    {
        private static char[,] _grid;

        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            var T = Convert.ToInt32(input);

            var k = 1;
            while (k <= T)
            {
                var caseParameters = Console.ReadLine().Split();
                var N = Convert.ToInt32(caseParameters[0]);
                var prePlacements = Convert.ToInt32(caseParameters[1]);

                InitializeEmptyGrid(N);

                for (var i = 0; i < prePlacements; i++)
                {
                    var modelDescriptor = Console.ReadLine().Split(' ');
                    var modelRow = Convert.ToInt32(modelDescriptor[1]);
                    var modelCol = Convert.ToInt32(modelDescriptor[2]);
                    PlaceModel(modelDescriptor[0][0], modelRow, modelCol);
                }

                var changes = GetChangesForBestScore(_grid);

                var stylePoints = ScoreGrid(_grid);
                Console.WriteLine($"Case #{k}: {stylePoints} {changes.Count}");

                foreach (var change in changes)
                {
                    Console.WriteLine(change);
                }

                k++;
            }
        }

        internal static IList<string> GetChangesForBestScore(char[,] grid)
        {
            var changes = new List<string>();

            var N = (int)Math.Sqrt(grid.Length);
            foreach (var coords in GenerateAllGridCoordinates(N))
            {
                var currentValue = grid[coords.Row, coords.Col];
                if (currentValue == 'o') continue;

                if (RowNotUsed(grid, coords.Row)
                    && ColNotUsed(grid, coords.Col)
                    && DiagonalNotUsed(grid, coords))
                {
                    grid[coords.Row, coords.Col] = 'o';
                    changes.Add($"o {coords.Row + 1} {coords.Col + 1}");
                }
                else if (currentValue != '.')
                {
                    continue;
                }
                else if (RowNotUsed(grid, coords.Row)
                         && ColNotUsed(grid, coords.Col))
                {
                    grid[coords.Row, coords.Col] = 'x';
                    changes.Add($"x {coords.Row + 1} {coords.Col + 1}");
                }
                else if (DiagonalNotUsed(grid, coords))
                {
                    grid[coords.Row, coords.Col] = '+';
                    changes.Add($"+ {coords.Row + 1} {coords.Col + 1}");
                }
            }

            return changes;
        }

        private static bool RowNotUsed(char[,] grid, int row)
        {
            var N = (int)Math.Sqrt(grid.Length);
            for (var j = 0; j < N; j++)
            {
                if (grid[row, j] == 'o' || grid[row, j] == 'x')
                    return false;
            }
            return true;
        }

        private static bool ColNotUsed(char[,] grid, int col)
        {
            var N = (int)Math.Sqrt(grid.Length);
            for (var i = 0; i < N; i++)
            {
                if (grid[i, col] == 'o' || grid[i, col] == 'x')
                    return false;
            }
            return true;
        }

        internal static bool DiagonalNotUsed(char[,] grid, GridCoordinates coords)
        {
            var N = (int)Math.Sqrt(grid.Length);
            for (var i = 0; i < N; i++)
            {
                var jFirstDiag = i - coords.Row + coords.Col;
                var jSecondDiag = coords.Row + coords.Col - i;
                if (jSecondDiag >= 0 && jSecondDiag < N
                    && (grid[i, jSecondDiag] == 'o' || grid[i, jSecondDiag] == '+'))
                    return false;
                if (jFirstDiag >= 0 && jFirstDiag < N
                    && (grid[i, jFirstDiag] == 'o' || grid[i, jFirstDiag] == '+'))
                    return false;
            }
            return true;
        }
        
        internal static int ScoreGrid(char[,] grid)
        {
            var score = 0;
            var N = Math.Sqrt(grid.Length);
            foreach (var coords in GenerateAllGridCoordinates((int)N))
            {
                if (grid[coords.Row, coords.Col] == '+' || grid[coords.Row, coords.Col] == 'x')
                    score += 1;
                else if (grid[coords.Row, coords.Col] == 'o')
                    score += 2;
            }
            return score;
        }

        private static void PlaceModel(char model, int modelRow, int modelCol)
        {
            _grid[modelRow - 1, modelCol - 1] = model;
        }

        private static void InitializeEmptyGrid(int N)
        {
            _grid = new char[N, N];
            foreach (var coords in GenerateAllGridCoordinates(N))
                _grid[coords.Row, coords.Col] = '.';
        }

        private static IEnumerable<GridCoordinates> GenerateAllGridCoordinates(int N)
        {
            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < N; j++)
                {
                    yield return new GridCoordinates
                    {
                        Row = i,
                        Col = j
                    };
                }
            }
        }

        internal static string GridToString(char[,] grid, int N)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < N; j++)
                {
                    sb.Append(grid[i, j]);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    internal class GridCoordinates
    {
        public int Row { get; set; }
        public int Col { get; set; }
    }
}
