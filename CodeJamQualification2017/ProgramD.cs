using System;
using System.Collections.Generic;
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
                    var modelDescriptor = Console.ReadLine();
                    var change = Change.Parse(modelDescriptor);
                    change.ApplyOnGrid(_grid);
                }

                var changes = GetChangesForBestScoreBruteForce(_grid, N);

                Console.WriteLine(GridToString(_grid, N));

                var stylePoints = ScoreGrid(_grid);
                Console.WriteLine($"Case #{k}: {stylePoints} {changes.Count}");

                foreach (var change in changes)
                {
                    Console.WriteLine(change);
                }

                k++;
            }
        }

        public static List<Change> GetChangesForBestScoreBruteForce(char[,] grid, int N)
        {
            Change bestChange = null;
            var bestDownstreamChanges = new List<Change>();
            var bestScore = 0;

            foreach (var change in ListAllValidMoves(grid, N))
            {
                change.ApplyOnGrid(grid);

                var clonedGrid = CloneGrid(grid, N);
                var changes = GetChangesForBestScoreBruteForce(clonedGrid, N);

                var stylePoints = ScoreGrid(clonedGrid);

                if (stylePoints > bestScore)
                {
                    bestChange = change;
                    bestScore = stylePoints;
                    bestDownstreamChanges = changes;
                }

                change.RevertOnGrid(grid);
            }

            if (bestChange != null)
            {
                bestDownstreamChanges.Insert(0, bestChange);

                foreach (var change in bestDownstreamChanges)
                {
                    change.ApplyOnGrid(grid);
                }
            }

            return bestDownstreamChanges;
        }

        private static IEnumerable<Change> ListAllValidMoves(char[,] grid, int N)
        {
            foreach (var coords in GenerateAllGridCoordinates(N))
            {
                var currentValue = grid[coords.Row, coords.Col];
                if (currentValue == 'o') continue;

                if (RowNotUsed(grid, coords)
                    && ColNotUsed(grid, coords)
                    && DiagonalNotUsed(grid, coords))
                {
                    yield return new Change
                    {
                        FromChar = currentValue,
                        ToChar = 'o',
                        Coords = coords
                    };
                }

                if (currentValue != '.')
                {
                    continue;
                }

                if (RowNotUsed(grid, coords) && ColNotUsed(grid, coords))
                {
                    yield return new Change
                    {
                        FromChar = currentValue,
                        ToChar = 'x',
                        Coords = coords
                    };
                }

                if (DiagonalNotUsed(grid, coords))
                {
                    yield return new Change
                    {
                        FromChar = currentValue,
                        ToChar = '+',
                        Coords = coords
                    };
                }
            }
        }

        private static char[,] CloneGrid(char[,] grid, int n)
        {
            var clone = new char[n, n];
            foreach (var coords in GenerateAllGridCoordinates(n))
                clone[coords.Row, coords.Col] = grid[coords.Row, coords.Col];
            return clone;
        }

        internal static IList<string> GetChangesForBestScore(char[,] grid)
        {
            var changes = new List<string>();

            var N = (int)Math.Sqrt(grid.Length);
            foreach (var coords in GenerateAllGridCoordinates(N))
            {
                var currentValue = grid[coords.Row, coords.Col];
                if (currentValue == 'o') continue;

                if (RowNotUsed(grid, coords)
                    && ColNotUsed(grid, coords)
                    && DiagonalNotUsed(grid, coords))
                {
                    grid[coords.Row, coords.Col] = 'o';
                    changes.Add($"o {coords.Row + 1} {coords.Col + 1}");
                }
                else if (currentValue != '.')
                {
                    continue;
                }
                else if (RowNotUsed(grid, coords)
                         && ColNotUsed(grid, coords))
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

        private static bool RowNotUsed(char[,] grid, GridCoordinates coords)
        {
            var N = (int)Math.Sqrt(grid.Length);
            for (var j = 0; j < N; j++)
            {
                if (j == coords.Col) continue;

                if (grid[coords.Row, j] == 'o' || grid[coords.Row, j] == 'x')
                    return false;
            }
            return true;
        }

        private static bool ColNotUsed(char[,] grid, GridCoordinates coords)
        {
            var N = (int)Math.Sqrt(grid.Length);
            for (var i = 0; i < N; i++)
            {
                if (i == coords.Row) continue;

                if (grid[i, coords.Col] == 'o' || grid[i, coords.Col] == 'x')
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
                    && jSecondDiag != coords.Col
                    && (grid[i, jSecondDiag] == 'o' || grid[i, jSecondDiag] == '+'))
                    return false;
                if (jFirstDiag >= 0 && jFirstDiag < N
                    && jFirstDiag != coords.Col
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

    internal class Change
    {
        public char FromChar { get; set; }
        public char ToChar { get; set; }
        public GridCoordinates Coords { get; set; }

        public override string ToString()
        {
            return $"{ToChar} {Coords.Row + 1} {Coords.Col + 1}";
        }

        public void ApplyOnGrid(char[,] grid)
        {
            grid[Coords.Row, Coords.Col] = ToChar;
        }

        public void RevertOnGrid(char[,] grid)
        {
            grid[Coords.Row, Coords.Col] = FromChar;
        }

        public static Change Parse(string descriptor)
        {
            var modelDescriptor = descriptor.Split(' ');
            var coords = new GridCoordinates
            {
                Row = Convert.ToInt32(modelDescriptor[1]) - 1,
                Col = Convert.ToInt32(modelDescriptor[2]) - 1
            };
            return new Change
            {
                ToChar = modelDescriptor[0][0],
                Coords = coords
            };
        }
    }

    internal class GridCoordinates
    {
        public int Row { get; set; }
        public int Col { get; set; }
    }
}
