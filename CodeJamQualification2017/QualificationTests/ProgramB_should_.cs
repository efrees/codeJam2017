using CodeJamQualification2017;
using NUnit.Framework;

namespace QualificationTests
{
    internal class ProgramB_should_
    {
        [TestCase("7", "7")]
        [TestCase("123", "123")]
        [TestCase("132", "129")]
        [TestCase("1000", "999")]
        [TestCase("111111111111111110", "99999999999999999")]
        [TestCase("101", "99")]
        [TestCase("1201", "1199")]
        public void solve_examples_correctly(string input, string expectedOutput)
        {
            var actualOutput = ProgramB.SolveCase(input);
            Assert.AreEqual(expectedOutput, actualOutput);
        }
    }
}
