using Project1.Logic;
using System.Security.Principal;
using System.Text;

namespace Project1.IO
{
    public class UI
    {
        public UI() { }

        public int WelcomeScreen()
        {
            Console.WriteLine("*-------------* Welcome to the app! *-------------*");
            Console.WriteLine("What would you like to do?\n[1] Login as Employee\n[2] Login as Manager\n[3] Register");
            int option;
            while (true)
            {
                if (!(Int32.TryParse(Console.ReadLine(), out option)) || option < 1 || option > 3)
                {
                    Console.WriteLine("Please enter '1', '2', or '3'");
                }
                else
                {
                    return option;
                }
            }
        }

        public void ShowOpenTickets(List<Ticket> tickets)
        {
            Console.WriteLine("Open Tickets:\n");
            foreach (Ticket ticket in tickets)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Ticket Number | {ticket.TicketNum}");
                sb.AppendLine($"Ticket Amount | {ticket.Amount}");
                sb.AppendLine($"Ticket Submit | {ticket.}");
                sb.AppendLine($"Ticket Number | {ticket.TicketNum}");
            }
        }

    }
}