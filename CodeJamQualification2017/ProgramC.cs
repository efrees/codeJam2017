using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeJamQualification2017
{
    class ProgramC
    {
        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            var T = Convert.ToInt32(input);

            var k = 1;
            while (k <= T)
            {
                var caseInput = Console.ReadLine().Split();
                var numStalls = Convert.ToInt64(caseInput[0]);
                var numPeople = Convert.ToInt64(caseInput[1]);
                var answer = SolveCase(numStalls, numPeople);

                Console.WriteLine($"Case #{k}: {answer}");
                k++;
            }
        }
        
        internal static string SolveCase(long numStalls, long numPeople)
        {
            var fullLevels = CountFullLevels(numPeople);
            var nextPowOf2 = (long)Math.Pow(2, fullLevels);

            var numPeopleInFullLevels = nextPowOf2 - 1;
            var numEmpty = numStalls - numPeopleInFullLevels;

            var sizeOfGapsOnNextLevel = numEmpty / nextPowOf2; //rounding down
            var howManyAreLargerGaps = numEmpty - (nextPowOf2 * sizeOfGapsOnNextLevel);
            var numPeopleForNextLevel = numPeople - numPeopleInFullLevels;

            var bestGap = numPeopleForNextLevel <= howManyAreLargerGaps ? sizeOfGapsOnNextLevel + 1 : sizeOfGapsOnNextLevel;

            var lastMin = (bestGap - 1) / 2;
            var lastMax = bestGap - lastMin - 1;
            return $"{lastMax} {lastMin}";
        }

        private static long CountFullLevels(long numPeople)
        {
            long pow = 1;
            int count = 0;
            while (pow < numPeople)
            {
                numPeople -= pow;
                pow *= 2;
                count++;
            }
            return count;
        }
    }
}
