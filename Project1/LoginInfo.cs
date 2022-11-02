﻿using System;
using System.Collections.Generic;

namespace Project1
{
    public class LoginInfo
    {
        public string username = "username";
        public string password = "password";
        Dictionary<string, string> loginInfo = new Dictionary<string, string>();

        public LoginInfo() { }

        public void Login()
        {
            string userTry;
            string passTry;
            while (true)
            {
                Console.WriteLine("Enter username:");
                userTry = Console.ReadLine();
                Console.WriteLine("Enter password:");
                passTry = Console.ReadLine();

                if (loginInfo.ContainsKey(userTry) && loginInfo[userTry] == passTry)
                {
                    break;
                }
                else if (!(loginInfo.ContainsKey(userTry)))
                {
                    Console.WriteLine("Username is incorrect.");
                    continue;
                }
                else if (!(loginInfo[username] == passTry))
                {
                    Console.WriteLine("Password is incorrect.");
                }
                else
                {
                    Console.WriteLine("Username or Password are incorrect");
                }
            }
            Console.WriteLine("Successfully logged in.");
        }


        public void Register()
        {
            Console.WriteLine("Enter a username:");
            username = Console.ReadLine();
            Console.WriteLine("Enter a password:");
            password = Console.ReadLine();

            loginInfo.Add(username, password);

            Console.WriteLine("Successfully registered.");
            Login();
        }
    }
}
/*
 ln 10         Dictionary<string, string> loginInfo;

        public LoginInfo()
        {
            loginInfo = new Dictionary<string, string>();
        }

        public void Login()
        {
            string userTry;
            string passTry;
            while (true)
            {
                Console.WriteLine("Enter username:");
                userTry = Console.ReadLine();

                if (loginInfo.ContainsKey(userTry))
                {
                    Console.WriteLine("Enter password:");
                    passTry = Console.ReadLine();

                    if (loginInfo[username] == passTry)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Password is incorrect.");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Username is incorrect.");
                    continue;
                }
*/