using System;

namespace Project1
{
    public class Project1
    {
        public static void Main()
        {
            LoginInfo loginInfo = new LoginInfo();

            while (true)
            {
                Console.WriteLine("Do you have an account?");
                if (Console.ReadLine().Equals("yes"))
                {
                    loginInfo.Login();
                }
                else
                {
                    loginInfo.Register();
                }
            }

        }
    }
}