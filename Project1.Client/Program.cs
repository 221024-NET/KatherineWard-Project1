using Microsoft.VisualBasic.FileIO;
using Project1.Client;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;

namespace Project1.Client
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static User? user;
        static bool running = true;

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("https://localhost:7158/"); //////// local url - can change w/ session
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            while (true)
            {
                user = null;
                await MainMenu();

                if (user != null)
                {
                    if (user.IsManager == true)
                    {
                        await ManagerMenu();
                    }
                    else
                    {
                        await EmployeeMenu();
                    }
                }
                else
                {
                    break;
                }
            }
            Environment.Exit(0);
        }

#region Menus
        static async Task<Boolean> MainMenu()
        {
            Console.Clear();
            Console.WriteLine("*-------------* Welcome to the app! *-------------*");
            Console.WriteLine("What would you like to do?\n[1] Login\n[2] Register as Employee\n[3] Register as Manager\n[4] Exit Application");
            int option;
            while (true)
            {
                if (!(Int32.TryParse(Console.ReadLine(), out option)) || option < 1 || option > 4)
                {
                    Console.WriteLine("Please enter 1, 2, 3, or 4");
                    continue;
                }
                else if (option == 1)
                {
                    user = await ConsoleLogin();
                    option = 0;
                    break;
                }
                else if (option == 2)
                {
                    User newUser = await ConsoleRegister();
                    user = await RegisterEmployee(newUser);
                    option = 0;
                    break;
                }
                else if (option == 3)
                {
                    User newUser = await ConsoleRegister();
                    user = await RegisterManager(newUser);
                    option = 0;
                    break;
                }
                else if (option == 4)
                {
                    break;
                }
            }
                return true;
        }

        static async Task<Boolean> ManagerMenu()
        {
            int option;

            Console.Clear();
            Console.WriteLine($"*-------------* Welcome {user.Name} *-------------*");

            while (true)
            {
                Console.WriteLine("What would you like to do?\n[1] View Pending Tickets\n[2] Manage Tickets\n[3] Log Out");
                if (!(Int32.TryParse(Console.ReadLine(), out option)) || option < 1 || option > 3)
                {
                    Console.WriteLine("Please enter '1' or '2'");
                }
                else if (option == 1)
                {
                    var pendingTickets = await ShowPendingTickets();

                    Console.WriteLine($"\nOpen Tickets: {pendingTickets.Count}\n");
                    ListTickets(pendingTickets);
                }
                else if (option == 2)
                {
                    var pendingTickets = await ShowPendingTickets();

                    Console.WriteLine($"\nOpen Tickets: {pendingTickets.Count}\n");
                    ListTickets(pendingTickets);

                    Ticket ticket = await UpdateTicket(pendingTickets);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"Ticket Number | {ticket.TicketNum}");
                    sb.AppendLine($"Submitted by  | {ticket.Name}");
                    sb.AppendLine($"Amount        | ${ticket.Amount}");
                    sb.AppendLine($"Description   | {ticket.Description}");
                    sb.AppendLine($"Status        | {ticket.Status}\n");

                    Console.Clear();
                    Console.WriteLine("Updated Ticket:\n");
                    Console.WriteLine(sb.ToString());
                }
                else if (option == 3)
                {
                    break;
                }
            }
                return true;
        }

        static async Task<Boolean> EmployeeMenu()
        {
            int option;

            Console.Clear();
            Console.WriteLine($"*-------------* Welcome {user.Name} *-------------*");

            while (true)
            {
                Console.WriteLine("What would you like to do?\n[1] Submit Ticket\n[2] View Previous Tickets\n[3] Log Out");
                if (!(Int32.TryParse(Console.ReadLine(), out option)) || option < 1 || option > 3)
                {
                    Console.WriteLine("Please enter 1, 2, or 3.");
                    continue;
                }
                else if (option == 1)
                {
                    Ticket newTicket = await CreateTicket();
                    newTicket.EmployeeId = user.EmployeeId;
                    Ticket addedTicket = await AddNewTicket(newTicket);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"Ticket Number | {addedTicket.TicketNum}");
                    sb.AppendLine($"Submitted by  | {addedTicket.Name}");
                    sb.AppendLine($"Amount        | ${addedTicket.Amount}");
                    sb.AppendLine($"Description   | {addedTicket.Description}");
                    sb.AppendLine($"Status        | {addedTicket.Status}\n");

                    Console.Clear();
                    Console.WriteLine("Added Ticket:\n");
                    Console.WriteLine(sb.ToString());
                }
                else if (option == 2)
                {
                    var previousTickets = await ShowPreviousTickets(user.EmployeeId);

                    Console.Clear();
                    Console.WriteLine($"Your Tickets: {previousTickets.Count}\n");
                    ListTickets(previousTickets);
                }
                else if (option == 3)
                {
                    break;
                }
            }
                return true;
        }
        #endregion


#region Logging In
        static async Task<User> GetUser(string username)
        {
            HttpResponseMessage response = await client.GetAsync($"user?username={username}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<User>();
            }
            else
            {
                return null;
            }
        }

        static async Task<User> ConsoleLogin()
        {
            User newUser = new User();
            while (true)
            {
                Console.WriteLine("Enter your username:");
                string username = Console.ReadLine();

                newUser = await GetUser(username);

                if (newUser.Username != username)
                {
                    Console.WriteLine("Wrong username. Try again.");
                    continue;
                }
                else
                {
                    Console.WriteLine("Enter your password:");
                    string password = Console.ReadLine();

                    if (newUser.Password != password)
                    {
                        Console.WriteLine("Wrong password. Try again.");
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Login Successful!");
                        break;
                    }
                }
            }
            return newUser;
        }
        #endregion


#region Registration
        static async Task<User> ConsoleRegister()
        {
            User newUser;
            string username;
            string password;
            string name;

            while (true)
            {
                Console.WriteLine("Enter a username:");
                username = Console.ReadLine();

                newUser = await GetUser(username);

                if (newUser.Username == null)
                {
                    Console.WriteLine("Enter a password:");
                    password = Console.ReadLine();
                    Console.WriteLine("Enter your full name:");
                    name = Console.ReadLine();
                    newUser.Username = username;
                    newUser.Password = password;
                    newUser.Name = name;
                    break;
                }
                else
                {
                    Console.WriteLine("Username taken. Please try another.");
                    continue;
                }
            }

            return newUser;
        }

        static async Task<User> RegisterEmployee(User user)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                $"registeremployee", user);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<User>();
        }

        static async Task<User> RegisterManager(User user)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                $"registermanager", user);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<User>();
        }
        #endregion


#region Showing Tickets
        static async Task<List<Ticket>> ShowPendingTickets()
        {
            List<Ticket> pendingTickets = new List<Ticket>();
            var path = "tickets";
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                pendingTickets = await response.Content.ReadAsAsync<List<Ticket>>();
            }
            return pendingTickets;
        }

        static async Task<List<Ticket>> ShowPreviousTickets(int employeeId)
        {
            List<Ticket> previousTickets = new List<Ticket>();
            var path = $"tickets/{employeeId}";
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                previousTickets = await response.Content.ReadAsAsync<List<Ticket>>();
            }
            return previousTickets;
        }

        static void ListTickets(List<Ticket> ticketList)
        {
            foreach (Ticket ticket in ticketList)
            {
                Console.WriteLine($"Ticket Number | {ticket.TicketNum}");
                Console.WriteLine($"Submitted by  | {ticket.Name}");
                Console.WriteLine($"Amount        | ${ticket.Amount}");
                Console.WriteLine($"Description   | {ticket.Description}");
                Console.WriteLine($"Status        | {ticket.Status}\n");
            }
        }
        #endregion


#region Creating Tickets
        static async Task<Ticket> CreateTicket()
        {
            Ticket newTicket = new Ticket();

            while (true)
            {
                Console.WriteLine("\nEnter the amount to be reimbursed:");
                decimal amount;
                if(Decimal.TryParse(Console.ReadLine(), out amount))
                {
                    newTicket.Amount = amount;
                }
                else
                {
                    Console.WriteLine("Please enter in decimal format (00.00).");
                    continue;
                }
                Console.WriteLine("\nEnter a description:");
                newTicket.Description = Console.ReadLine();
                break;
            }
            return newTicket;
        }

        static async Task<Ticket> AddNewTicket(Ticket newTicket)
        {
            Ticket addedTicket = new Ticket();
            HttpResponseMessage response = await client.PostAsJsonAsync(
                $"tickets/{newTicket.EmployeeId}", newTicket);

            if (response.IsSuccessStatusCode)
            {
                addedTicket = await response.Content.ReadAsAsync<Ticket>();
            }

            return addedTicket;
        }
        #endregion


#region Managing Tickets
        static async Task<Ticket> UpdateTicket(List<Ticket> pendingTickets)
        {
            int ticketNum;
            int employeeId = 0;
            int statusCheck;
            Ticket tempTicket;

            Console.WriteLine("\nWhich ticket would you like to manage? (Enter ID):");

            while (true)
            {
                if (!(Int32.TryParse(Console.ReadLine(), out ticketNum)) || !(pendingTickets.Exists(x => x.TicketNum == ticketNum)))
                {
                    Console.WriteLine("Please enter a valid ticket id number.");
                    continue;
                }
                else
                {
                    tempTicket = pendingTickets.Find(x => x.TicketNum == ticketNum);
                    employeeId = tempTicket.EmployeeId;
                    break;
                }
            }

            Ticket ticketToUpdate = await GetTicket(employeeId, ticketNum);


            while (true)
            {
                Console.WriteLine($"What would you like to do?\n[1] Approve\n[2] Deny\n");

                if (!(Int32.TryParse(Console.ReadLine(), out statusCheck)) || statusCheck < 1 || statusCheck > 2)
                {
                    Console.WriteLine("Please enter 1 or 2.");
                    continue;
                }
                else if (statusCheck == 1)
                {
                    ticketToUpdate.Status = "Approved";
                    break;
                }
                else if (statusCheck == 2)
                {
                    ticketToUpdate.Status = "Denied";
                    break;
                }
            }
                    return await ManageTickets(ticketToUpdate);
        }

        static async Task<Ticket> GetTicket(int employeeId, int ticketNum)
        {
            HttpResponseMessage response = await client.GetAsync($"tickets/{employeeId}/{ticketNum}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Ticket>();
            }
            else
            {
                return null;
            }
        }

        static async Task<Ticket> ManageTickets(Ticket updatedTicket)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"tickets/{updatedTicket.EmployeeId}/{updatedTicket.TicketNum}", updatedTicket);

            if (response.IsSuccessStatusCode)
            {
                updatedTicket = await response.Content.ReadAsAsync<Ticket>();
            }

            return updatedTicket;
        }
        #endregion

    }
}