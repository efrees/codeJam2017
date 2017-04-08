using System;
using System.Diagnostics;
using CodeJamQualification2017;
using NUnit.Framework;

namespace QualificationTests
{
    internal class ProgramC_should_
    {
        [TestCase(4, 4, "0 0")]
        [TestCase(1000, 1000, "0 0")]
        [TestCase(4, 2, "1 0")]
        [TestCase(5, 2, "1 0")]
        [TestCase(6, 2, "1 1")]
        [TestCase(1000, 1, "500 499")]
        [TestCase(1000, 498, "1 0")]
        [TestCase(64, 16, "2 1")]
        public void solve_examples_correctly(int numStalls, int numPeople, string expectedAnswer)
        {
            var actualAnswer = ProgramC.SolveCase(numStalls, numPeople);
            Assert.AreEqual(expectedAnswer, actualAnswer);
        }
        
        [TestCase(1000000, 999999, "0 0")]
        [TestCase(1000000, 499998, "1 0")]
        [TestCase(1000000, 99999, "7 6")]
        [TestCase(1000000000000000000, 99999, "7629394531249 7629394531249")]
        public void run_fast_for_large_inputs(long numStalls, int numPeople, string expectedAnswer)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var actualAnswer = ProgramC.SolveCase(numStalls, numPeople);
            stopwatch.Stop();
            Assert.AreEqual(expectedAnswer, actualAnswer);
            Assert.That(stopwatch.Elapsed, Is.LessThan(TimeSpan.FromSeconds(45)));
        }
    }
}
