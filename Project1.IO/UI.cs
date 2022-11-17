using System.Security.Principal;
using System.Text;
using Project1.Logic;

namespace Project1.IO
{
    public class UI
    {
        public UI() { }

        public int WelcomeScreen()
        {
            Console.WriteLine("*-------------* Welcome to the app! *-------------*");
            Console.WriteLine("What would you like to do?\n[1] Login as Employee\n[2] Register as Employee\n[3] Register as Manager");
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

        public int MainMenu(User user)
        {
            int option;
            Console.WriteLine($"\n*-------------* Welcome {user.Name} *-------------*");

            if (user.IsManager)
            {
                while (true)
                {
                    Console.WriteLine("What would you like to do?\n[1] View Pending Tickets\n[2] Manage Tickets");
                    if (!(Int32.TryParse(Console.ReadLine(), out option)) || option < 1 || option > 2)
                    {
                        Console.WriteLine("Please enter '1' or '2'");
                    }
                    else
                    {
                        return option;
                    }
                }
            }
            else
            {
                while (true)
                {
                    Console.WriteLine("What would you like to do?\n[1] Submit Ticket\n[2] View Previous Tickets");
                    if (!(Int32.TryParse(Console.ReadLine(), out option)) || option < 1 || option > 2)
                    {
                        Console.WriteLine("Please enter '1' or '2'");
                    }
                    else
                    {
                        return option + 2;
                    }
                }
            }

        }

        public void ShowOpenTickets(List<Ticket> tickets)
        {
            Console.WriteLine("Open Tickets:\n");
            StringBuilder sb = new StringBuilder();
            foreach (Ticket ticket in tickets)
            {
                sb.AppendLine($"Ticket Number | {ticket.TicketNum}");
                sb.AppendLine($"Submitted by  | {ticket.Name}");  ////// employee name
                sb.AppendLine($"Amount        | ${ticket.Amount}");
                sb.AppendLine($"Description   | {ticket.Description}\n");
            }
            Console.WriteLine(sb.ToString());
        }

        public void ShowPreviousTickets(List<Ticket> tickets)
        {
            Console.WriteLine("Previous Tickets:\n");
            StringBuilder sb = new StringBuilder();
            foreach (Ticket ticket in tickets)
            {
                sb.AppendLine($"Ticket Number | {ticket.TicketNum}");
                sb.AppendLine($"Amount        | ${ticket.Amount}");
                sb.AppendLine($"Description   | {ticket.Description}");
                sb.AppendLine($"Status        | {ticket.Status}\n");  ////// employee name
            }
            Console.WriteLine(sb.ToString());
        }

    }
}