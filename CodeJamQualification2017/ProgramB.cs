using System;
using System.Linq;

namespace CodeJamQualification2017
{
    class ProgramB
    {
        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            var T = Convert.ToInt32(input);

            var k = 1;
            while (k <= T)
            {
                var caseInput = Console.ReadLine();

                var answer = SolveCase(caseInput);

                Console.WriteLine($"Case #{k}: {answer}");
                k++;
            }
        }

        internal static string SolveCase(string caseInput)
        {
            var digitsArray = caseInput.Select(c => c - '0').ToArray();
            for (int i = digitsArray.Length - 2; i >= 0; i--)
            {
                if (digitsArray[i] <= digitsArray[i + 1])
                    continue;

                if (digitsArray[i] > 0)
                {
                    digitsArray[i]--;
                    FillNines(digitsArray, i + 1);
                }
            }

            var stringFromDigits = new string(digitsArray.Select(c => (char)(c + '0')).ToArray());
            return stringFromDigits.TrimStart('0');
        }

        private static void FillNines(int[] digitsArray, int start)
        {
            for (int i = start; i < digitsArray.Length; i++)
            {
                digitsArray[i] = 9;
            }
        }
    }
}
