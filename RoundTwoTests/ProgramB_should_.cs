using NUnit.Framework;
using RoundTwo;

namespace RoundTwoTests
{
    internal class ProgramB_should_
    {
        [Test]
        public void solve_customer_constrained_case()
        {
            var tickets = new[]
            {
                new Ticket {Customer = 1, Seat = 1},
                new Ticket {Customer = 1, Seat = 2},
                new Ticket {Customer = 1, Seat = 1},
            };

            var answer = ProgramB.ProcessTestCase(tickets, 2);
            Assert.AreEqual("3 0", answer);
        }

        [Test]
        public void solve_seat_constrained_case()
        {
            var tickets = new[]
            {
                new Ticket {Customer = 1, Seat = 1},
                new Ticket {Customer = 2, Seat = 1},
                new Ticket {Customer = 1, Seat = 1},
            };

            var answer = ProgramB.ProcessTestCase(tickets, 2);
            Assert.AreEqual("3 0", answer);
        }

        [Test]
        public void solve_overlap_case()
        {
            var tickets = new[]
            {
                new Ticket {Customer = 1, Seat = 1},
                new Ticket {Customer = 2, Seat = 1},
                new Ticket {Customer = 1, Seat = 2},
            };

            var answer = ProgramB.ProcessTestCase(tickets, 2);
            Assert.AreEqual("2 0", answer);
        }

        [Test]
        public void solve_promotion_case()
        {
            var tickets = new[]
            {
                new Ticket {Customer = 1, Seat = 2},
                new Ticket {Customer = 2, Seat = 2},
                new Ticket {Customer = 1, Seat = 2},
            };

            var answer = ProgramB.ProcessTestCase(tickets, 2);
            Assert.AreEqual("2 1", answer);
        }

        [Test]
        public void solve_example_4_correctly()
        {
            var tickets = new[]
            {
                new Ticket {Customer = 2, Seat = 3},
                new Ticket {Customer = 1, Seat = 2},
                new Ticket {Customer = 3, Seat = 3},
                new Ticket {Customer = 1, Seat = 3},
            };

            var answer = ProgramB.ProcessTestCase(tickets, 2);
            Assert.AreEqual("2 1", answer);
        }

        [Test]
        public void avoid_early_promotion()
        {
            var tickets = new[]
            {
                new Ticket {Customer = 1, Seat = 1},
                new Ticket {Customer = 1, Seat = 2},
                new Ticket {Customer = 1, Seat = 3},
                new Ticket {Customer = 2, Seat = 2},
                new Ticket {Customer = 2, Seat = 2},
            };

            var answer = ProgramB.ProcessTestCase(tickets, 2);
            Assert.AreEqual("3 0", answer);
        }

        [Test]
        public void choose_match_that_avoids_later_promotion()
        {
            var tickets = new[]
            {
                new Ticket {Customer = 1, Seat = 1},
                new Ticket {Customer = 1, Seat = 2},
                new Ticket {Customer = 1, Seat = 3},
                new Ticket {Customer = 2, Seat = 1},
                new Ticket {Customer = 2, Seat = 2},
                new Ticket {Customer = 2, Seat = 3},
            };

            var answer = ProgramB.ProcessTestCase(tickets, 2);
            Assert.AreEqual("3 0", answer);
        }

        [Test]
        public void choose_match_that_avoids_later_promotion2()
        {
            var tickets = new[]
            {
                new Ticket {Customer = 1, Seat = 1},
                new Ticket {Customer = 1, Seat = 1},
                new Ticket {Customer = 1, Seat = 2},
                new Ticket {Customer = 2, Seat = 3},
                new Ticket {Customer = 2, Seat = 3},
                new Ticket {Customer = 2, Seat = 2},
            };

            var answer = ProgramB.ProcessTestCase(tickets, 2);
            Assert.AreEqual("3 0", answer);
        }
    }
}
