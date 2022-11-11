using Project1.Data;
using Project1.IO;
using Project1.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.App
{
    public class MainApp
    {
        IRepository _repo;
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
                    isManager = false;
                    break;
                case 2:
                    Login();
                    isManager = true;
                    break;
                case 3:
                    Register();
                    break;
            }
        }

        public void GetOpenTickets()
        {
            List<Ticket> openTickets = _repo.GetOpenTickets();


        }


        public void Login()
        {
            while (true)
            {
                Console.WriteLine("Enter your username:");
                string username = Console.ReadLine();
                Console.WriteLine("Enter your password:");
                string password = Console.ReadLine();

                if (_repo.GetLogin(username, password))
                {
                    Console.WriteLine("Login successful!");
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

            while (true)
            {
                Console.WriteLine("Register a username:");
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

            if (_repo.Register(username, password, false))
            {
                Console.WriteLine("Successfully registered!");
            }
        }
    }
}
