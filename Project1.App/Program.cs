using Project1.Data;
using Project1.IO;
using Project1.Logic;
using System.Security.Principal;

namespace Project1.App
{
    public class Project1
    {
        static void Main()
        {
            bool isManager = false;
            int loginType = 0;

            string connectionString = File.ReadAllText(@"/Revature/221024/Project1/ConnectionStrings/Project1ConnectionString.txt");
            IRepository repo = new SqlRepository(connectionString);

            UI ui = new UI();
            Account account = new Account();

            loginType = ui.BeginApp();

            switch (loginType)
            {
                case 1:
                    account.Login();
                    isManager = false;
                    break;
                case 2:
                    account.Login();
                    isManager = true;
                    break;
                case 3:
                    account.Register();
                    break;
            }

        }
    }
}