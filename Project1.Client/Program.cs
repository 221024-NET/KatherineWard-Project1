﻿using Project1.Client;
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
            User user;

        #region [Console] Initial Login

            Console.WriteLine("*-------------* Welcome to the app! *-------------*");
            Console.WriteLine("What would you like to do?\n[1] Login\n[2] Register as Employee\n[3] Register as Manager");
            int option;
            while (true)
            {
                if (!(Int32.TryParse(Console.ReadLine(), out option)) || option < 1 || option > 3)
                {
                    Console.WriteLine("Please enter '1', '2', or '3'");
                }
                else if (option == 1)
                {
                    user = await ConsoleLogin();
                    Console.WriteLine($"Welcome {user.Name}");
                    option = 0;
                    break;
                }
                else if (option == 2)
                {
                    user = await ConsoleRegister();
                    Console.WriteLine($"User created at {RegisterEmployee(user)}");
                    option = 0;
                    break;
                }
                else if (option == 3)
                {
                    user = await ConsoleRegister();
                    Console.WriteLine($"User created at {RegisterManager(user)}");
                    option = 0;
                    break;
                }
            }
            #endregion

            Console.Clear();
            Console.WriteLine($"*-------------* Welcome {user.Name} *-------------*");

        #region [Console] Manager Ticket Menu

            if (user.IsManager)
            {
                while (true)
                {
                    Console.WriteLine("What would you like to do?\n[1] View Pending Tickets\n[2] Manage Tickets");
                    if (!(Int32.TryParse(Console.ReadLine(), out option)) || option < 1 || option > 2)
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

                        Console.WriteLine("Updated Ticket:\n");
                        Console.WriteLine(sb.ToString());
                    }
                }
            }
            #endregion


        #region [Console] Employee Ticket Menu

            else
            {
                while (true)
                {
                    Console.WriteLine("What would you like to do?\n[1] Submit Ticket\n[2] View Previous Tickets");
                    if (!(Int32.TryParse(Console.ReadLine(), out option)) || option < 1 || option > 2)
                    {
                        Console.WriteLine("Please enter '1' or '2'");
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

                        Console.WriteLine("Added Ticket:\n");
                        Console.WriteLine(sb.ToString());
                    }
                    else if (option == 2)
                    {
                        var previousTickets = await ShowPreviousTickets(user.EmployeeId);

                        Console.WriteLine($"\nYour Tickets: {previousTickets.Count}\n");
                        ListTickets(previousTickets);
                    }
                }
            }
#endregion

        }

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
            User user;
            while (true)
            {
                Console.WriteLine("Enter your username:");
                string username = Console.ReadLine();

                user = await GetUser(username);

                if (user.EmployeeId == 0)
                {
                    Console.WriteLine("Wrong username. Try again.");
                    continue;
                }
                else
                {
                    Console.WriteLine("Enter your password:");
                    string password = Console.ReadLine();

                    if (user.Password != password)
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
            return user;
        }
        #endregion


#region Registration
        static async Task<User> ConsoleRegister()
        {
            User user;
            string username;
            string password;
            string name;

            while (true)
            {
                Console.WriteLine("Enter a username:");
                username = Console.ReadLine();

                user = await GetUser(username);

                if (user.Username == null)
                {
                    Console.WriteLine("Enter a password:");
                    password = Console.ReadLine();
                    Console.WriteLine("Enter your full name:");
                    name = Console.ReadLine();
                    user.Username = username;
                    user.Password = password;
                    user.Name = name;
                    break;
                }
                else
                {
                    Console.WriteLine("Username taken. Please try another.");
                    continue;
                }
            }

            return user;
        }

        static async Task<Uri> RegisterEmployee(User user)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                $"registeremployee", user);
            response.EnsureSuccessStatusCode();

            return response.Headers.Location;
        }

        static async Task<Uri> RegisterManager(User user)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                $"registermanager", user);
            response.EnsureSuccessStatusCode();

            return response.Headers.Location;
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
                Console.WriteLine("Enter the amount to be reimbursed:");
                decimal amount;
                if(Decimal.TryParse(Console.ReadLine(), out amount))
                {
                    newTicket.Amount = amount;
                }
                else
                {
                    Console.WriteLine("Please enter as \"00.00\"");
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

            Console.WriteLine("\nWhich ticket would you like to manage? (Enter ID):");
            Int32.TryParse(Console.ReadLine(), out ticketNum);

            foreach(Ticket ticket in pendingTickets)
            {
                if (ticket.TicketNum == ticketNum)
                {
                    employeeId = ticket.EmployeeId;
                    break;
                }
            }

            Ticket ticketToUpdate = await GetTicket(employeeId, ticketNum);


            while (true)
            {
                Console.WriteLine($"What would you like to do?\n[1] Approve\n[2] Deny");

                if (!(Int32.TryParse(Console.ReadLine(), out statusCheck) || statusCheck < 1 || statusCheck > 2))
                {
                    Console.WriteLine("Please enter '1' or '2'.");
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