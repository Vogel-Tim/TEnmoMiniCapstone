﻿using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoClient.Services;
using TenmoClient.ApiServices;

namespace TenmoClient
{
    class Program
    {
        private static readonly ConsoleService consoleService = new ConsoleService();
        private static readonly AuthService authService = new AuthService();
        private static readonly AccountApiService accountApiService = new AccountApiService();
        private static readonly TransferApiService transferApiService = new TransferApiService();
        private static readonly UserApiService userApiService = new UserApiService();

        static void Main(string[] args)
        {
            Run();
        }

        private static void Run()
        {
            while(true)
            {
                int loginRegister = -1;
                while (loginRegister != 1 && loginRegister != 2)
                {
                    Console.WriteLine("Welcome to TEnmo!");
                    Console.WriteLine("1: Login");
                    Console.WriteLine("2: Register");
                    Console.Write("Please choose an option: ");

                    if (!int.TryParse(Console.ReadLine(), out loginRegister))
                    {
                        Console.WriteLine("Invalid input. Please enter only a number.");
                    }
                    else if (loginRegister == 1)
                    {
                        while (!UserService.IsLoggedIn()) //will keep looping until user is logged in
                        {
                            LoginUser loginUser = consoleService.PromptForLogin();
                            ApiUser user = authService.Login(loginUser);
                            if (user != null)
                            {
                                UserService.SetLogin(user);
                            }
                        }
                    }
                    else if (loginRegister == 2)
                    {
                        bool isRegistered = false;
                        while (!isRegistered) //will keep looping until user is registered
                        {
                            LoginUser registerUser = consoleService.PromptForLogin();
                            isRegistered = authService.Register(registerUser);
                            if (isRegistered)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Registration successful. You can now log in.");
                                loginRegister = -1; //reset outer loop to allow choice for login
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection.");
                    }
                }

                MenuSelection();
            }
        }

        private static void MenuSelection()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                //Console.Clear();
                Console.WriteLine("");
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (menuSelection == 1)
                {
                    decimal balance = accountApiService.GetAccount(UserService.GetUserId()).Balance;
                    Console.WriteLine($"Your current account balance is: {balance:C2}");
                }
                else if (menuSelection == 2)
                {
                    string fromOrTo = "";
                    List<Transfer> transfers = transferApiService.GetTransfers();
                    foreach(Transfer transfer in transfers)
                    {                                       //Determines whether console writes from or to for transfer
                        fromOrTo = transfer.AccountFrom == accountApiService.GetAccount(UserService.GetUserId()).Id ? "From" : "To"; 
                                                            //Determines username depending on whether currently logged in user is account_from or to
                        string username = fromOrTo == "From" ? accountApiService.GetUserAccount(transfer.AccountTo).Username : accountApiService.GetUserAccount(transfer.AccountFrom).Username; 
                        Console.WriteLine($"{transfer.Id}, {fromOrTo}: {username}, {transfer.Amount}");
                    }

                    Console.WriteLine($"Choose a transfer to view details or exit");
                }
                else if (menuSelection == 3)
                {
                    
                }
                else if (menuSelection == 4)
                {
                    List<ApiUser> users = userApiService.GetUsers();
                    foreach (ApiUser user in users)
                    {
                        if (user.UserId != UserService.GetUserId())
                        {
                            Console.WriteLine($"{user.UserId}: {user.Username}");
                        }
                    }

                    Transfer transfer = consoleService.PromptForSendToId();
                    if(accountApiService.GetAccount(UserService.GetUserId()).Balance >= transfer.Amount)
                    {
                        transferApiService.CreateTransfer(transfer);
                        transferApiService.Transaction(transfer);
                       
                    }
                    else
                    {
                        Console.WriteLine("Transfer declined due to insufficient funds.");
                    }
                }
                else if (menuSelection == 5)
                {

                }
                else if (menuSelection == 6)
                {
                    Console.WriteLine("");
                    UserService.SetLogin(new ApiUser()); //wipe out previous login info
                    Console.Clear();
                    menuSelection = 0;
                }
                else
                {
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                }
            }
        }

    }
}
