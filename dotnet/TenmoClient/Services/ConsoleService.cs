using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoClient.ApiServices;

namespace TenmoClient.Services
{
    public class ConsoleService
    {

        private bool _validInput = false;
        private int _userTo = 0;
        private decimal _amount = 0;
        UserApiService _userApiService = new UserApiService();

        /// <summary>
        /// Prompts for transfer ID to view, approve, or reject
        /// </summary>
        /// <param name="action">String to print in prompt. Expected values are "Approve" or "Reject" or "View"</param>
        /// <returns>ID of transfers to view, approve, or reject</returns>
        public int PromptForTransferID(string action)
        {
            Console.WriteLine("");
            Console.Write("Please enter transfer ID to " + action + " (0 to cancel): ");
            if (!int.TryParse(Console.ReadLine(), out int auctionId))
            {
                Console.WriteLine("Invalid input. Only input a number.");
                return 0;
            }
            else
            {
                return auctionId;
            }
        }

        public LoginUser PromptForLogin()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            string password = GetPasswordFromConsole("Password: ");

            LoginUser loginUser = new LoginUser
            {
                Username = username,
                Password = password
            };
            return loginUser;
        }

        private string GetPasswordFromConsole(string displayMessage)
        {
            string pass = "";
            Console.Write(displayMessage);
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (!char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Remove(pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine("");
            return pass;
        }

        public void DisplayUsers()
        {            
            List<ApiUser> users = _userApiService.GetUsers();
            foreach (ApiUser user in users)
            {
                if (user.UserId != UserService.GetUserId())
                {
                    Console.WriteLine($"{user.UserId}: {user.Username}");
                }
            }
        }

        public Transfer BuildTransactionSend(int userId, decimal amount)
        {
            AccountApiService apiService = new AccountApiService();
            Transfer transfer = new Transfer();
               
            transfer.AccountTo = apiService.GetAccount(userId).Id;
            transfer.Amount = amount;
            transfer.TypeId = 2;
            transfer.StatusId = 2;
            transfer.AccountFrom = apiService.GetAccount(UserService.GetUserId()).Id;
            return transfer;
        }

        //public Transfer PromptForTransactionReceive(int userId)
        //{
        //    AccountApiService apiService = new AccountApiService();
        //    Transfer transfer = new Transfer();


        //        else
        //    {
        //        transfer.AccountTo = apiService.GetAccount(userTo).Id;
        //    }


        //        else
        //    {
        //        transfer.Amount = amount;
        //    }

        //    transfer.TypeId = 2;
        //    transfer.StatusId = 2;
        //    transfer.AccountFrom = apiService.GetAccount(UserService.GetUserId()).Id;
        //    return transfer;
        //}

        public int PromptForTransactionUserId()
        {
            List<ApiUser> users = _userApiService.GetUsers();
            _validInput = false;
            do
            {
                Console.Write($"Enter ID of the User: ");
                if (!int.TryParse(Console.ReadLine(), out int user))
                {
                    Console.WriteLine("Invalid input! Please enter a valid user ID.");
                }
                else
                {
                    _userTo =  user;
                    foreach (ApiUser apiUser in users)
                    {
                        if (apiUser.UserId == _userTo && apiUser.UserId != UserService.GetUserId())
                        {
                            _validInput = true;
                        }
                    }
                    if (!_validInput)
                    {
                        Console.WriteLine("Cannot send money to that User! Please enter a valid user ID.");
                    }
                }
            } while (_validInput == false);
            return _userTo;
        }

        public decimal PromptForTransactionAmount()
        {
            _validInput = false;
            do
            {
                Console.Write("Enter amount to transfer: $");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
                {
                    Console.WriteLine("Invalid input! Please enter a valid dollar amount greater than 0.");
                }
                else
                {
                    _amount = amount;
                    _validInput = true;
                }
            } while (_validInput == false);
            return _amount;
        }

    }
}
