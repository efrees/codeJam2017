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
            if (numStalls == numPeople)
            {
                return "0 0";
            }

            var orderedGaps = new List<long> { numStalls };

            var lastMax = 0L;
            var lastMin = 0L;

            while (numPeople > 0)
            {
                var bestGap = orderedGaps.First();
                orderedGaps.RemoveAt(0);

                lastMin = (bestGap - 1) / 2;
                lastMax = bestGap - lastMin - 1;

                InsertOrdered(orderedGaps, lastMax);
                InsertOrdered(orderedGaps, lastMin);
                numPeople--;
            }

            return $"{lastMax} {lastMin}";
        }

        private static void InsertOrdered(List<long> orderedGapsDesc, long numToInsert)
        {
            int i = 0;
            while (i < orderedGapsDesc.Count && orderedGapsDesc[i] > numToInsert)
            {
                i++;
            }

            orderedGapsDesc.Insert(i, numToInsert);
        }
    }
}
