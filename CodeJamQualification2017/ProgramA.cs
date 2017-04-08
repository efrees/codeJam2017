using System;
using System.Linq;

namespace CodeJamQualification2017
{
    class ProgramA
    {
        static void MainA(string[] args)
        {
            var input = Console.ReadLine();
            var T = Convert.ToInt32(input);

            var k = 1;
            while (k <= T)
            {
                var caseInput = Console.ReadLine().Split(' ');
                var pancakeRow = caseInput[0].ToCharArray();
                var flipperSize = Convert.ToInt32(caseInput[1]);

                var answer = SolveCase(pancakeRow, flipperSize);

                Console.WriteLine($"Case #{k}: {answer}");
                k++;
            }
        }

        internal static string SolveCase(char[] pancakeRow, int flipperSize)
        {
            var answer = "IMPOSSIBLE";

            int flipCount = 0;
            for (int i = 0; i <= pancakeRow.Length - flipperSize; i++)
            {
                if (pancakeRow[i] == '-')
                {
                    flipCount++;
                    FlipCakes(pancakeRow, i, flipperSize);
                }
            }

            if (pancakeRow.All(c => c == '+'))
            {
                answer = flipCount.ToString();
            }
            return answer;
        }

        internal static void FlipCakes(char[] pancakeRow, int start, int flipperSize)
        {
            for (int i = 0; i < flipperSize; i++)
            {
                pancakeRow[start + i] = pancakeRow[start + i] == '-' ? '+' : '-';
            }
        }
    }
}
