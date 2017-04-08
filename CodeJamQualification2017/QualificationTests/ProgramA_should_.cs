using CodeJamQualification2017;
using NUnit.Framework;

namespace QualificationTests
{
    internal class ProgramA_should_
    {
        [Test]
        public void flip_row_correctly()
        {
            var initialRow = "--+-++";
            var start = 2;
            var size = 3;
            var charArray = initialRow.ToCharArray();
            ProgramA.FlipCakes(charArray, start, size);

            Assert.AreEqual("---+-+", new string(charArray));
        }

        [Test]
        public void solve_first_example_correctly()
        {
            var pancakeRow = "---+-++-";
            var actualAnswer = ProgramA.SolveCase(pancakeRow.ToCharArray(), 3);
            Assert.AreEqual("3", actualAnswer);
        }
    }
}
