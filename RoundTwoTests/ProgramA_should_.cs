using NUnit.Framework;
using RoundTwo;

namespace RoundTwoTests
{
    public class ProgramA_should_
    {
        [Test]
        public void solve_case_correctly()
        {
            var gList = new[] { 4, 1, 1, 1, 1, 1 };
            var p = 2;

            var answer = ProgramA.ProcessTestCase(gList, p);

            Assert.AreEqual(4, answer);
        }

        [Test]
        public void solve_3_case_correctly()
        {
            var gList = new[] { 3, 1, 1, 1, 1, 1 };
            var p = 3;

            var answer = ProgramA.ProcessTestCase(gList, p);

            Assert.AreEqual(3, answer);
        }

        [Test]
        public void solve_4_case_correctly()
        {
            var p = 3;
            var gList = new[] { 4, 1, 1, 1, 1, 1, 2, 2 };

            var answer = ProgramA.ProcessTestCase(gList, p);
            Assert.AreEqual(4, answer);

            gList = new[] { 4, 1, 3, 1, 1, 1, 2, 2 };

            answer = ProgramA.ProcessTestCase(gList, p);
            Assert.AreEqual(4, answer);

            gList = new[] { 4, 1, 3, 1, 1, 1, 1, 2, 2 };

            answer = ProgramA.ProcessTestCase(gList, p);
            Assert.AreEqual(5, answer);

            gList = new[] { 4, 1, 3, 1, 1, 1, 1, 2, 2, 2 };

            answer = ProgramA.ProcessTestCase(gList, p);
            Assert.AreEqual(5, answer);

        }
    }
}
