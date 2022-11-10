using System;

namespace Project1
{
    public class Project1
    {
        public void Main1()
        {
            LoginInfo loginInfo = new LoginInfo();
            bool isManager = false;

            Console.WriteLine("What would you like to do?\n1) Login as Employee\n2) Login as Manager\n3) Register");
            int option;
            while (true)
            {
                if (!(Int32.TryParse(Console.ReadLine(), out option)) || option < 1 || option > 3)
                {
                    Console.WriteLine("Please enter '1' or '2'");
                }
                else if (option == 1)
                {
                    loginInfo.Login();
                    break;
                }
                else if (option == 2)
                {
                    loginInfo.Login();
                    isManager = true;
                    break;
                }
                else if (option == 3)
                {
                    loginInfo.Register();
                    break;
                }
            }

            if (!isManager)
            {
                Console.WriteLine("What would you like to do?\n1) Submit Ticket\n2) View Previous Tickets");
            }
            else
            {
                Console.WriteLine("What would you like to do?\n1) View Tickets\n2) View Previous Tickets");
            }
        }
    }
}