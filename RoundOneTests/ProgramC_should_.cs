using NUnit.Framework;
using RoundOne;

namespace RoundOneTests
{
    internal class ProgramC_should_
    {
        [Test]
        public void solve_example()
        {
            var parameters = new[] { "98", "1", "99", "49", "0", "1" };
            var answer = ProgramC.ProcessTestCaseInternal(parameters);
            Assert.AreEqual("172", answer);
        }
    }
}
