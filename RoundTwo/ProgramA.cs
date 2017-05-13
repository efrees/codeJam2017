using System;
using System.Linq;

namespace RoundTwo
{
    internal class ProgramA
    {
        static void MainA(string[] args)
        {
            var input = Console.ReadLine();
            var T = Convert.ToInt32(input);

            var k = 1;
            while (k <= T)
            {
                var parameters = Console.ReadLine().Split(' ');
                var N = Convert.ToInt32(parameters[0]);
                var P = Convert.ToInt32(parameters[1]);
                var gList = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

                var answer = ProcessTestCase(gList, P);

                Console.WriteLine($"Case #{k}: {answer}");
                k++;
            }
        }

        internal static int ProcessTestCase(int[] gList, int p)
        {
            var n = gList.Length;
            var simplifiedList = gList.Select(i => i % p).ToList();

            var zeros = simplifiedList.Count(i => i == 0);
            var ones = simplifiedList.Count(i => i == 1);
            var twos = simplifiedList.Count(i => i == 2);
            var threes = simplifiedList.Count(i => i == 3);

            if (p == 4)
            {
                var pairs = Math.Min(ones, threes) + twos / 2;
                var remnantOnesOrThrees = Math.Max(ones, threes) - pairs;
                var remnantTwos = twos % 2 == 0 ? 0 : 1;

                var triples = Math.Min(remnantOnesOrThrees / 2, remnantTwos);

                remnantOnesOrThrees -= triples * 2;
                remnantTwos = 0;
                
                var quads = remnantOnesOrThrees / 4;

                remnantOnesOrThrees -= quads * 4;

                return zeros
                       + pairs
                       + triples
                       + quads
                       + Math.Min(1, remnantOnesOrThrees);
            }
            else if (p == 3)
            {
                var pairs = Math.Min(ones, twos);
                var remnant = Math.Max(ones, twos) - pairs;

                var triples = remnant / 3;

                remnant = remnant - (triples * 3);

                return zeros
                       + pairs
                       + triples
                       + Math.Min(1, remnant);
            }
            else //p == 2
            {
                var pairs = ones / 2;
                var remnant = ones - (pairs * 2);

                return zeros
                       + pairs
                       + Math.Min(1, remnant);
            }
        }
    }
}
