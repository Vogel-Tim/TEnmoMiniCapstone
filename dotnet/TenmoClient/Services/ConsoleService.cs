using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoClient.ApiServices;

namespace TenmoClient.Services
{
    public class ConsoleService
    {
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

        public Transfer PromptForSendToId()
        {
            AccountApiService apiService = new AccountApiService();
            Transfer transfer = new Transfer();
            bool validInput = true;
            
            do
            {
                Console.Write("Enter ID of user to send TE bucks to: ");
                if (!int.TryParse(Console.ReadLine(), out int userTo))
                {
                    Console.WriteLine("Invalid input! Please enter a valid user ID.");
                    validInput = false;
                }
                else
                {

                    transfer.AccountTo = apiService.GetAccount(userTo).Id;
                }
                Console.WriteLine("Enter amount to transfer: $");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
                {
                    Console.WriteLine("Invalid input! Please enter a valid dollar amount");
                    validInput = false;
                }
                else
                {
                    transfer.Amount = amount;
                }
            } while (validInput == false);

            transfer.TypeId = 2;
            transfer.StatusId = 2;
            transfer.AccountFrom = apiService.GetAccount(UserService.GetUserId()).Id;
            return transfer;

        }
    }
}
