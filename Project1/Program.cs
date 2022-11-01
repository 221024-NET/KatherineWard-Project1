using System;

public class Project1
{
    public static string username = "username";
    public static string password = "password";

    public static void Main()
    {
        Console.WriteLine("Do you have an account?");
        if (Console.ReadLine().Equals("yes"))
        {
            Login();
        }
        else
        {
            Register();
        }
    }

    public static void Login()
    {
        string userTry;
        string passTry;
        while (true)
        {
            Console.WriteLine("Enter username:");
            userTry = Console.ReadLine();
            Console.WriteLine("Enter password:");
            passTry = Console.ReadLine();

            if (userTry == username && passTry == password)
            {
                break;
            }
            else
            {
                Console.WriteLine("Username or Password were incorrect.");
                continue;
            }
        }
        Console.WriteLine("Successfully logged in.");
    }


    public static void Register()
    {
        Console.WriteLine("Enter a username:");
        username = Console.ReadLine();
        Console.WriteLine("Enter a password:");
        password = Console.ReadLine();
        Console.WriteLine("Successfully registered.");
    }

}
