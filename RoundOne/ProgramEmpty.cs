using System;

namespace RoundOne
{
    internal class ProgramEmpty
    {
        static void MainE(string[] args)
        {
            var input = Console.ReadLine();
            var T = Convert.ToInt32(input);

            var k = 1;
            while (k <= T)
            {
                var answer = ProcessTestCase();

                Console.WriteLine($"Case #{k}: {answer}");
                k++;
            }
        }

        internal static string ProcessTestCase()
        {
            return "TODO";
        }
    }
}
