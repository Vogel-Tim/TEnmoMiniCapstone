using System;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoClient.ApiServices;
using System.Threading;

namespace TenmoClient.Services
{
    public class ConsoleService
    {

        private bool _validInput = false;
        UserApiService _userApiService = new UserApiService();
        AccountApiService _accountApiService = new AccountApiService();
        TransferApiService _transferApiService = new TransferApiService();

        private const int REQUEST_TYPE_ID = 1;
        private const int REQUEST_STATUS_ID = 1;
        private const int SEND_TYPE_ID = 2;
        private const int SEND_STATUS_ID = 2;

        /// <summary>
        /// Prompts for transfer ID to view, approve, or reject
        /// </summary>
        /// <param name="action">String to print in prompt. Expected values are "approve/reject" or "view"</param>
        /// <returns>ID of transfers to view, approve, or reject</returns>
        public int PromptForTransferID(string action)
        {
            _validInput = false;
            int _transferId = 0;
            bool isPending = action == "approve/reject";
            List<Transfer> transfers = _transferApiService.GetTransfers(isPending);
            do
            {
                Console.WriteLine("");
                Console.Write("Please enter transfer ID to " + action + " (0 to cancel): ");
                if (!int.TryParse(Console.ReadLine(), out int transferId))
                {
                    Console.WriteLine("Invalid input. Only input a number.");
                }
                else if (transferId == 0)
                {
                    return 0;
                }
                else
                {
                    _transferId = transferId;
                    foreach (Transfer transfer in transfers)
                    {
                        if (transfer.Id == _transferId)
                        {
                            _validInput = true;
                            break;
                        }
                    }
                    if (_validInput == false)
                    {
                        Console.WriteLine("Please select a valid transaction ID to view.");
                    }
                }
            } while (_validInput == false);

            return _transferId;
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

        public void PrintUserHeaderForSend()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("|        SEND TO       |");
            Console.WriteLine("------------------------");
            Console.WriteLine("| ID      USER         |");
            Console.WriteLine("------------------------");
        }

        public void PrintUserHeaderForRequest()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("|     REQUEST FROM     |");
            Console.WriteLine("------------------------");
            Console.WriteLine("| ID      USER         |");
            Console.WriteLine("------------------------");
        }

        public void DisplayUsers()
        {
            List<ApiUser> users = _userApiService.GetUsers();
            foreach (ApiUser user in users)
            {
                if (user.UserId != UserService.GetUserId())
                {
                    Console.WriteLine($"  {user.UserId}:   {user.Username}");
                }
            }
        }

        public Transfer BuildTransactionSend(int userId, decimal amount)
        {
            Transfer transfer = new Transfer();
               
            transfer.AccountTo = _accountApiService.GetAccount(userId).Id;
            transfer.Amount = amount;
            transfer.TypeId = SEND_TYPE_ID;
            transfer.StatusId = SEND_STATUS_ID;
            transfer.AccountFrom = _accountApiService.GetAccount(UserService.GetUserId()).Id;
            return transfer;
        }

        public Transfer BuildTransactionRequest(int userId, decimal amount)
        {            
            Transfer transfer = new Transfer();

            transfer.AccountTo = _accountApiService.GetAccount(UserService.GetUserId()).Id;
            transfer.Amount = amount;
            transfer.TypeId = REQUEST_TYPE_ID;
            transfer.StatusId = REQUEST_STATUS_ID;
            transfer.AccountFrom = _accountApiService.GetAccount(userId).Id;
            return transfer;
        }

        public int PromptForTransactionUserId()
        {
            Console.WriteLine("");
            List<ApiUser> users = _userApiService.GetUsers();
            _validInput = false;
            int _userTo = 0;
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
            decimal _amount = 0;
            do
            {
                Console.Write("Enter amount to transfer: $");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount.CompareTo(.01M) < 0)
                {
                    Console.WriteLine("Invalid input! Please enter a valid dollar amount greater than $0.01");
                }
                else
                {
                    _amount = amount;
                    _validInput = true;
                }
            } while (_validInput == false);
            return _amount;
        }

        public bool PrintPastOrPendingTransfers(bool isPending)
        {
            string fromOrTo = "";
            string isRejected;
            List<Transfer> transfers = _transferApiService.GetTransfers(isPending);
            if (transfers != null && transfers.Count != 0)
            {
                DisplayTEnmoLogo();
                PrintTransfersHeader();
                foreach (Transfer transfer in transfers)
                {
                    isRejected = "";
                    //Determines whether console writes from or to for transfer
                    fromOrTo = transfer.AccountFrom == _accountApiService.GetAccount(UserService.GetUserId()).Id ? "To" : "From";
                    //Determines username depending on whether currently logged in user is account_from or to
                    string username = fromOrTo == "To" ? _accountApiService.GetUserAccount(transfer.AccountTo).Username : _accountApiService.GetUserAccount(transfer.AccountFrom).Username;
                    if (transfer.StatusId == 3)
                    {
                        isRejected = "(Rejected)";
                    }
                    PrintTransfers(transfer.Id, fromOrTo, username, transfer.Amount, isRejected);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void PrintTransfers(int id, string fromOrTo, string username, decimal amount, string isRejected)
        {
            
            for(int i = 0; i <= 25; i++)
            {
                if (i == 2)
                {
                    Console.Write(id);
                }
                else if (i == 9)
                {
                    Console.Write(fromOrTo + ": ");
                    if (fromOrTo.Length == 2)
                    {
                        if (username.Length > 11)
                        {
                            username = username.Substring(0, 11);
                        }
                        Console.Write($"  {username}");
                        i += username.Length;
                    }
                    else
                    {
                        if (username.Length > 11)
                        {
                            username = username.Substring(0, 11);
                        }
                        Console.Write(username);
                        i += username.Length;
                    }
                }
                else if (i == 25)
                {
                    Console.Write($"{amount:C2} {isRejected}");
                }
                else
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine("");
            
        }

        public void PrintTransfersHeader()
        {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("|               TRANSFERS               |");
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("| ID        FROM/TO              AMOUNT |");
            Console.WriteLine("-----------------------------------------");
        }

        public void SingleTransferHeader(int transfer)
        {
            Console.WriteLine( "----------------------------");
            Console.WriteLine($"|   TRANSFER {transfer} DETAILS  |");
            Console.WriteLine("----------------------------");
        }
        public void PrintTransferDetails(Transfer transfer)
        {
            DisplayTEnmoLogo();
            SingleTransferHeader(transfer.Id);
            Console.WriteLine($"Id:     {transfer.Id}");
            Console.WriteLine($"From:   {_accountApiService.GetUserAccount(transfer.AccountFrom).Username}");
            Console.WriteLine($"To:     {_accountApiService.GetUserAccount(transfer.AccountTo).Username}");
            if (transfer.TypeId == 1)
            {
                Console.WriteLine($"Type:   Request");
            }
            else if (transfer.TypeId == 2)
            {
                Console.WriteLine($"Type:   Send");
            }
            if (transfer.StatusId == 1)
            {
                Console.WriteLine($"Status: Pending");
            }
            else if (transfer.StatusId == 2)
            {
                Console.WriteLine($"Status: Approved");
            }
            else if (transfer.StatusId == 3)
            {
                Console.WriteLine($"Status: Rejected");
            }
            Console.WriteLine($"Amount: {transfer.Amount:C2}");
            Console.WriteLine("");
            Console.WriteLine("Please hit ENTER to continue.");
            Console.ReadLine();
        }

        public string PromptForApproval()
        {
            Console.WriteLine("1: Approve");
            Console.WriteLine("2: Reject");
            Console.WriteLine("0: Exit");
            Console.WriteLine("-----------");
            Console.Write("Choose a response to requested transfer: ");

            string userChoice = Console.ReadLine();
            return userChoice;
        }

        public Transfer BuildUpdatedTransfer(bool isApproved, Transfer transferToUpdate)
        {
            Transfer updatedTransfer = transferToUpdate;
            if (isApproved)
            {
                updatedTransfer.StatusId = 2;
            }
            else
            {
                updatedTransfer.StatusId = 3;
            }
            return updatedTransfer;
        }

        public void DisplayBalance(decimal balance)
        {
            
            DisplayTEnmoLogo();
            Console.WriteLine("");
            Console.WriteLine($"Your current account balance is: {balance:C2}");
            Console.WriteLine("Please Wait...");
            Thread.Sleep(5000);
            Console.Clear();
        }

        public void DisplayTEnmoLogo()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.WriteLine(" __________________       ____________ ");
            Console.WriteLine("|                  |     |            |");
            Console.WriteLine("|______	     ______|     |    ________|");
            Console.WriteLine("       |    |            |   |         ");
            Console.WriteLine("       |    |            |   |________     ______________     _______________     _______________ ");
            Console.WriteLine("       |    |            |            |   |              |   |               |   |               |");
            Console.WriteLine("       |    |            |    ________|   |    ______    |   |   ___   ___   |   |     _____     |");
            Console.WriteLine("       |    |            |   |            |    |    |    |   |   | |   | |   |   |    |     |    |");
            Console.WriteLine("       |    |            |   |________    |    |    |    |   |   | |   | |   |   |    |_____|    |");
            Console.WriteLine("       |    |            |            |   |    |    |    |   |   | |   | |   |   |               |");
            Console.WriteLine("       |____|            |____________|   |____|    |____|   |___| |___| |___|   |_______________|");
            Console.WriteLine("");
        }
    }
}
