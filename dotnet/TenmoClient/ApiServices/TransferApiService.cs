using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using TenmoClient.Models;
using TenmoClient.Services;
using System.Threading;

namespace TenmoClient.ApiServices
{
    public class TransferApiService
    {
        private readonly static string API_URL = "https://localhost:44315/transfers/";
        private readonly IRestClient client = new RestClient();
        private readonly ApiUser user = new ApiUser();

        public bool LoggedIn
        {
            get
            {
                return !string.IsNullOrWhiteSpace(user.Token);
            }
        }

        public TransferApiService()
        {

        }

        public TransferApiService(IRestClient restClient)
        {
            client = restClient;
        }

        public Transfer GetTransfer(int transferId)
        {
            RestRequest request = new RestRequest($"{API_URL}{transferId}");
            request.AddHeader("Authorization", "Bearer " + UserService.GetToken());
            IRestResponse<Transfer> response = client.Get<Transfer>(request);
            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Transfer does not exist.");
                    Thread.Sleep(2000);
                    return null;
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                return response.Data;
            }
        }

        public List<Transfer> GetTransfers(bool isPending)
        {
            RestRequest request = new RestRequest($"{API_URL}account/{UserService.GetUserId()}");
            request.AddHeader("Authorization", "Bearer " + UserService.GetToken());
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);
            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("You have no past transfers.");
                    Thread.Sleep(2000);
                    return null;
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                List<Transfer> allTransfers = response.Data;
                List<Transfer> desiredTransfers = new List<Transfer>();
                if (isPending)
                {
                    foreach(Transfer transfer in allTransfers)
                    {
                        if (transfer.StatusId == 1)
                        {
                            desiredTransfers.Add(transfer);
                        }
                    }
                }
                else
                {
                    foreach (Transfer transfer in allTransfers)
                    {
                        if (transfer.StatusId != 1)
                        {
                            desiredTransfers.Add(transfer);
                        }
                    }
                }
                if (desiredTransfers.Count == 0)
                {
                    Console.WriteLine("You have no pending transfers right now.");
                    Thread.Sleep(2000);
                }
                return desiredTransfers;
            }
        }

        public bool UpdateTransfer(Transfer transfer)
        {
            RestRequest request = new RestRequest($"{API_URL}request");
            request.AddHeader("Authorization", "Bearer " + UserService.GetToken());
            request.AddJsonBody(transfer);
            IRestResponse response = client.Put(request);
            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CreateTransfer(Transfer transfer)
        {
            RestRequest request = new RestRequest($"{API_URL}transfer");
            request.AddHeader("Authorization", "Bearer " + UserService.GetToken());
            request.AddJsonBody(transfer);
            IRestResponse response = client.Post(request);
            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool Transaction(Transfer transfer)
        {
            RestRequest request = new RestRequest($"{API_URL}transfer");
            request.AddHeader("Authorization", "Bearer " + UserService.GetToken());
            request.AddJsonBody(transfer);
            IRestResponse response = client.Put(request);
            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
