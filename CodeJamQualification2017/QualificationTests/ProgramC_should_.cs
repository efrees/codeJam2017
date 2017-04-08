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
        public void solve_examples_correctly(int numStalls, int numPeople, string expectedAnswer)
        {
            var actualAnswer = ProgramC.SolveCase(numStalls, numPeople);
            Assert.AreEqual(expectedAnswer, actualAnswer);
        }
    }
}
