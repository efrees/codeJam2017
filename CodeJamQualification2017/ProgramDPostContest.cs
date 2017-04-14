using System;
using System.Collections.Generic;
using System.Text;

namespace CodeJamQualification2017
{
    class ProgramDPostContest
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

                var optimalSolution = GetOptimalSolution(_grid);

                var stylePoints = ScoreGrid(optimalSolution);
                var changes = GetChangesFromInitial(_grid, optimalSolution);

                Console.WriteLine($"Case #{k}: {stylePoints} {changes.Count}");

                foreach (var change in changes)
                {
                    Console.WriteLine(change);
                }

                k++;
            }
        }

        internal static char[,] GetOptimalSolution(char[,] grid)
        {
            var rookSolution = SolveRookSubProblem(grid);
            var bishopSolution = SolveBishopSubProblem(grid);
            var mergedSolution = MergeSubProblemSolutions(rookSolution, bishopSolution);
            return mergedSolution;
        }

        internal static char[,] SolveRookSubProblem(char[,] initialGrid)
        {
            var N = (int)Math.Sqrt(initialGrid.Length);
            var gridCopy = new char[N, N];
            foreach (var coord in GenerateAllGridCoordinates(N))
            {
                var isRook = initialGrid[coord.Row, coord.Col] == 'o' || initialGrid[coord.Row, coord.Col] == 'x';
                gridCopy[coord.Row, coord.Col] = isRook ? 'x' : '.';
            }

            foreach (var coord in GenerateAllGridCoordinates(N))
            {
                if (RowAndColNotUsed(gridCopy, coord))
                {
                    gridCopy[coord.Row, coord.Col] = 'x';
                }
            }

            return gridCopy;
        }

        internal static char[,] SolveBishopSubProblem(char[,] initialGrid)
        {
            var N = (int)Math.Sqrt(initialGrid.Length);
            var gridCopy = new char[N, N];
            foreach (var coord in GenerateAllGridCoordinates(N))
            {
                var isBishop = initialGrid[coord.Row, coord.Col] == 'o' || initialGrid[coord.Row, coord.Col] == '+';
                gridCopy[coord.Row, coord.Col] = isBishop ? '+' : '.';
            }

            //Shortest diagonals first
            for (var i = N - 1; i >= 0; i--)
            {
                for (var j = 0; j + i < N; j++)
                {
                    var diagCoord = new GridCoordinates { Row = j, Col = j + i };
                    if (DiagonalNotUsed(gridCopy, diagCoord))
                    {
                        gridCopy[diagCoord.Row, diagCoord.Col] = '+';
                        break;
                    }
                }

                for (var j = 0; j + i < N; j++)
                {
                    var diagCoord = new GridCoordinates { Row = N - j - 1, Col = N - i - 1 };
                    if (DiagonalNotUsed(gridCopy, diagCoord))
                    {
                        gridCopy[diagCoord.Row, diagCoord.Col] = '+';
                        break;
                    }
                }
            }

            return gridCopy;
        }

        internal static char[,] MergeSubProblemSolutions(char[,] rookSolution, char[,] bishopSolution)
        {
            var N = (int)Math.Sqrt(rookSolution.Length);
            foreach (var coords in GenerateAllGridCoordinates(N))
            {
                rookSolution[coords.Row, coords.Col] = MergeCell(rookSolution[coords.Row, coords.Col],
                    bishopSolution[coords.Row, coords.Col]);
            }
            return rookSolution;
        }

        private static char MergeCell(char c1, char c2)
        {
            if (c1 == '.') return c2;
            if (c2 == '.') return c1;
            return 'o';
        }

        internal static IList<string> GetChangesFromInitial(char[,] initialGrid, char[,] finalSolution)
        {
            var changes = new List<string>();
            var N = (int)Math.Sqrt(initialGrid.Length);
            foreach (var coords in GenerateAllGridCoordinates(N))
            {
                if (finalSolution[coords.Row, coords.Col] != initialGrid[coords.Row, coords.Col])
                {
                    changes.Add($"{finalSolution[coords.Row, coords.Col]} {coords.Row + 1} {coords.Col + 1}");
                }
            }
            return changes;
        }

        private static bool RowAndColNotUsed(char[,] grid, GridCoordinates coords)
        {
            var N = (int)Math.Sqrt(grid.Length);
            for (var i = 0; i < N; i++)
            {
                if (grid[coords.Row, i] == 'o' || grid[coords.Row, i] == 'x'
                   || grid[i, coords.Col] == 'o' || grid[i, coords.Col] == 'x')
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
}
