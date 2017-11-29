using System;
using System.Collections.Generic;
using System.Linq;

namespace RoundTwo
{
    internal class ProgramB
    {
        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            var T = Convert.ToInt32(input);

            var k = 1;
            while (k <= T)
            {
                var parameters = Console.ReadLine().Split(' ');
                var N = Convert.ToInt32(parameters[0]);
                var C = Convert.ToInt32(parameters[1]);
                var ticketCount = Convert.ToInt32(parameters[2]);

                var ticketList = new List<Ticket>();
                while (ticketCount > 0)
                {
                    ticketCount--;
                    var ticketDetails = Console.ReadLine().Split(' ');
                    ticketList.Add(new RoundTwo.Ticket
                    {
                        Seat = Convert.ToInt32(ticketDetails[0]),
                        Customer = Convert.ToInt32(ticketDetails[1])
                    });
                }

                var answer = ProcessTestCase(ticketList, N);

                Console.WriteLine($"Case #{k}: {answer}");
                k++;
            }
        }

        internal static string ProcessTestCase(IList<Ticket> tickets, int seatCount)
        {
            //number of rides necessary
            //number of promotions to make that work
            var rideCount = 0;
            var promotionCount = 0;

            //small case
            var ticketsByCustomer = tickets.GroupBy(t => t.Customer).ToDictionary(g => g.Key, g => g.ToList());

            if (ticketsByCustomer.Count == 1)
            {
                return $"{ticketsByCustomer.First().Value.Count} 0";
            }

            var c1Key = ticketsByCustomer.First().Key;
            var c2Key = ticketsByCustomer.Last().Key;

            var orderedC2Tickets = ticketsByCustomer.ContainsKey(c2Key)
                ? ticketsByCustomer[c2Key].OrderByDescending(t => t.Seat).ToList()
                : new List<Ticket>();

            //Attempt to maximize matches (without promotion)
            var orderedC1Tickets = ticketsByCustomer[c1Key].OrderBy(t => t.Seat).ToList();
            foreach (var c2Ticket in orderedC2Tickets)
            {
                orderedC1Tickets.ForEach(t => t.TriedCurrentTicket = false);
                if (MatchC2ToC1(orderedC1Tickets, c2Ticket))
                {
                    rideCount++;
                }
            }

            var unmatchedC1Tickets = orderedC1Tickets.Where(t => t.MatchedWithTicket == null);
            foreach (var c1Ticket in unmatchedC1Tickets)
            {
                //could only be unmatched because the only c2's left were for the same seat.
                // if these are promotable, we can fill the ride better.
                if (orderedC2Tickets.Any())
                {
                    var promotion = orderedC2Tickets.Where(t => t.MatchedWithTicket == null).FirstOrDefault(t => t.Seat > 1);

                    if (promotion != null)
                    {
                        promotionCount++;
                        promotion.MatchedWithTicket = c1Ticket;
                    }
                }
            }

            rideCount += orderedC2Tickets.Where(t => t.MatchedWithTicket == null).Count()
                + orderedC1Tickets.Where(t => t.MatchedWithTicket == null).Count();

            return $"{rideCount} {promotionCount}";
        }

        private static bool MatchC2ToC1(List<Ticket> c1Tickets, Ticket c2Ticket)
        {
            //find a place to fulfil this ticket without promotion
            foreach (var c1Ticket in c1Tickets)
            {
                if (c2Ticket.Seat != c1Ticket.Seat && !c1Ticket.TriedCurrentTicket)
                {
                    c1Ticket.TriedCurrentTicket = true;

                    if (c1Ticket.MatchedWithTicket == null || MatchC2ToC1(c1Tickets, c1Ticket.MatchedWithTicket))
                    {
                        c1Ticket.MatchedWithTicket = c2Ticket;
                        c2Ticket.MatchedWithTicket = c1Ticket;
                        return true;
                    }
                }
            }
            return false;
        }
    }

    internal class Ticket
    {
        public int Seat { get; set; }
        public int Customer { get; set; }
        public Ticket MatchedWithTicket { get; set; }
        public bool TriedCurrentTicket { get; set; }
    }
}
