using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoClient.Services;
using TenmoClient.ApiServices;
using System.Threading;
using System.Transactions;


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
                    consoleService.DisplayTEnmoLogo();
                    Console.WriteLine("");
                    Console.WriteLine("Welcome to TEnmo!");
                    Console.WriteLine("1: Login");
                    Console.WriteLine("2: Register");
                    Console.Write("Please choose an option (0 to exit): ");

                    if (!int.TryParse(Console.ReadLine(), out loginRegister))
                    {
                        Console.WriteLine("Invalid input. Please enter only a number.");
                        Thread.Sleep(2000);
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
                            else
                            {
                                Console.Write("Press ENTER to try again or ENTER 0 to exit to login menu.");
                                if (Console.ReadLine() == "0")
                                {
                                    loginRegister = 0;
                                    break;
                                }
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
                                Thread.Sleep(2000);
                                loginRegister = -1; //reset outer loop to allow choice for login
                            }
                        }
                    }
                    else if (loginRegister == 0)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection.");
                        Thread.Sleep(2000);
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
                consoleService.DisplayTEnmoLogo();
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
                    menuSelection = -1;
                }
                else if (menuSelection == 1)
                {
                    decimal balance = accountApiService.GetAccount(UserService.GetUserId()).Balance;
                    consoleService.DisplayBalance(balance);
                }
                else if (menuSelection == 2)
                {
                    if (consoleService.PrintPastOrPendingTransfers(false))
                    {
                        int transferId = consoleService.PromptForTransferID("view");
                        if (transferId != 0)
                        {
                            Transfer transfer = transferApiService.GetTransfer(transferId);
                            consoleService.PrintTransferDetails(transfer);
                        }
                    }
                }
                else if (menuSelection == 3)
                {
                    if (consoleService.PrintPastOrPendingTransfers(true))
                    {
                        int transferId = consoleService.PromptForTransferID("approve/reject");
                        if (transferId != 0)
                        {
                            Transfer newTransfer = new Transfer();
                            Transfer existingTransfer = transferApiService.GetTransfer(transferId);
                            if (!(existingTransfer.AccountTo == accountApiService.GetAccount(UserService.GetUserId()).Id))
                            {
                                string userChoice = consoleService.PromptForApproval();

                                switch (userChoice)
                                {
                                    case "1":
                                        if (accountApiService.GetAccount(UserService.GetUserId()).Balance >= existingTransfer.Amount)
                                        {
                                            newTransfer = consoleService.BuildUpdatedTransfer(true, existingTransfer);
                                            TransactionScope transaction = new TransactionScope();
                                            if (transferApiService.Transaction(newTransfer))
                                            {
                                                transferApiService.UpdateTransfer(newTransfer);
                                                transaction.Complete();
                                                Console.WriteLine("Transfer approved. Transaction successful!");
                                                Thread.Sleep(2000);
                                            }
                                            else
                                            {
                                                transaction.Dispose();
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Insufficient funds. Transfer not approved.");
                                            Thread.Sleep(2000);
                                        }
                                        break;
                                    case "2":
                                        newTransfer = consoleService.BuildUpdatedTransfer(false, existingTransfer);
                                        transferApiService.UpdateTransfer(newTransfer);
                                        Console.WriteLine("Transfer rejected.");
                                        Thread.Sleep(2000);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Please wait for user to accept or reject this request for funds.");
                                Thread.Sleep(2000);
                            }
                        }
                    }
                }
                else if (menuSelection == 4)
                {
                    consoleService.DisplayTEnmoLogo();
                    consoleService.PrintUserHeaderForSend();
                    consoleService.DisplayUsers();
                    int userTo = consoleService.PromptForTransactionUserId();
                    decimal amount = consoleService.PromptForTransactionAmount();
                    Transfer transfer = consoleService.BuildTransactionSend(userTo, amount);
                    if (accountApiService.GetAccount(UserService.GetUserId()).Balance >= transfer.Amount)
                    {
                        TransactionScope transaction = new TransactionScope();
                        if (transferApiService.CreateTransfer(transfer) && transferApiService.Transaction(transfer))
                        {
                            transaction.Complete();
                            Console.WriteLine("Transaction successfull.");
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            transaction.Dispose();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Transfer declined due to insufficient funds.");
                        Thread.Sleep(2000);
                    }
                }
                else if (menuSelection == 5)
                {
                    consoleService.DisplayTEnmoLogo();
                    consoleService.PrintUserHeaderForRequest();
                    consoleService.DisplayUsers();
                    int userFrom = consoleService.PromptForTransactionUserId();
                    decimal amount = consoleService.PromptForTransactionAmount();
                    Transfer transfer = consoleService.BuildTransactionRequest(userFrom, amount);
                    if (transferApiService.CreateTransfer(transfer))
                    {
                        Console.WriteLine("Request sent.");
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.WriteLine("Request unsuccessful.");
                        Thread.Sleep(2000);
                    }
                }
                else if (menuSelection == 6)
                {
                    Console.WriteLine("");
                    UserService.SetLogin(new ApiUser()); //wipe out previous login info
                    menuSelection = 0;
                }
                else if(menuSelection == 0)
                {
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again.");
                    Thread.Sleep(2000);
                }
            }
        }

    }
}
