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
            MainApp mainApp = new MainApp();

            string connectionString = File.ReadAllText(@"/Revature/221024/Project1/ConnectionStrings/Project1ConnectionString.txt");
            IRepository repo = new SqlRepository(connectionString);


            mainApp.MainLogin();

        }
    }
}