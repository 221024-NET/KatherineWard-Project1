using Project1.Data;
using Project1.IO;
using Project1.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project1.App
{
    public class MainApp
    {
        IRepository _repo;
        UI ui = new UI();
        User user;
        bool isManager = false;
        int loginType = 0;

        public MainApp() { }

        public MainApp(IRepository repo)
        {
            this._repo = repo;
        }

        public void MainLogin()
        {
            UI ui = new UI();

            int loginType = ui.WelcomeScreen();

            switch (loginType)
            {
                case 1:
                    Login();
                    break;
                case 2:
                    Register();
                    isManager = true;
                    break;
                case 3:
                    Register();
                    isManager = true;
                    break;
            }
        }
        public void MainMenu()
        {
            UI ui = new UI();

            int loginType = ui.MainMenu(user);

            switch (loginType)
            {
                case 1: // view pending tickets
                    GetOpenTickets();
                    break;
                case 2: // manage tickets
                    ManageTickets();
                    break;
                case 3: // submit ticket

                    break;
                case 4: // view previous tickets
                    GetPreviousTickets();
                    break;
            }
        }


        public User Login()
        {
            while (true)
            {
                Console.WriteLine("Enter your username:");
                string username = Console.ReadLine();
                Console.WriteLine("Enter your password:");
                string password = Console.ReadLine();

                if (_repo.CheckLogin(username, password))
                {
                    user = _repo.GetUser(username);
                    Console.WriteLine("Login successful!");
                    return user;
                }
                else
                {
                    Console.WriteLine("Username or password incorrect. Please try again.\n");
                    continue;
                }
            }

        }

        public void Register()
        {
            string username;
            string password;
            string name;

            if (isManager)
            {
                Console.WriteLine("Enter manager credentials:");
                if (Console.ReadLine().ToLower() == "please") isManager = true;
                else
                {
                    Console.WriteLine("Wrong credentials. Returning to Main.");
                    isManager = false;
                    MainLogin();
                    return;
                }
            }


            while (true)
            {
                Console.WriteLine("/nRegister a username:");
                username = Console.ReadLine();

                if (_repo.CheckUsername(username))
                {
                    Console.WriteLine("Username taken, please try another.\n");
                    continue;
                }
                else
                {
                    break;
                }
            }

            while (true)
            {
                Console.WriteLine("Register a password:");
                password = Console.ReadLine();

                Console.WriteLine("Re-enter password:");
                string confirmPass = Console.ReadLine();

                if (password == confirmPass)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Passwords do not match. Try again.\n");
                    Thread.Sleep(500);
                    continue;
                }

            }

            Console.WriteLine("Please enter your full name:");
            name = Console.ReadLine();

            if (isManager)
            {
                if (_repo.ManagerRegister(username, password, name))
                {
                    user = _repo.GetUser(username);
                    Console.WriteLine("Successfully registered!");
                }
            }
            else
            {
                if (_repo.EmployeeRegister(username, password, name))
                {
                    user = _repo.GetUser(username);
                    Console.WriteLine("Successfully registered!");
                }
            }

        }

        public void GetOpenTickets()
        {
            List<Ticket> openTickets = _repo.GetOpenTickets();
            ui.ShowOpenTickets(openTickets);
        }
        public void GetPreviousTickets()
        {
            List<Ticket> pastTickets = _repo.GetPreviousTickets(user.EmployeeId);
            ui.ShowPreviousTickets(pastTickets);
        }
        public void ManageTickets()
        {
            List<Ticket> openTickets = _repo.GetOpenTickets();
            ui.ShowPreviousTickets(openTickets);
            int ticketNum;
            int statusCheck;

            Console.WriteLine("Which ticket would you like to manage? (Enter ID):");
            Int32.TryParse(Console.ReadLine(), out ticketNum);

            Console.WriteLine($"What would you like to do?\n[1] Approve\n[2] Deny");
            Int32.TryParse(Console.ReadLine(), out statusCheck);

            Ticket ticket = _repo.ManageTicket(ticketNum, statusCheck);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Ticket Number | {ticket.TicketNum}");
            sb.AppendLine($"Submitted by  | {ticket.Name}");  ////// employee name
            sb.AppendLine($"Amount        | ${ticket.Amount}");
            sb.AppendLine($"Description   | {ticket.Description}");
            sb.AppendLine($"Status        | {ticket.Status}\n");
        
            Console.WriteLine(sb.ToString());
        }
    }
}
