using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using TenmoClient.Models;
using TenmoClient.Services;

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

        public List<Transfer> GetTransfers()
        {
            RestRequest request = new RestRequest($"{API_URL}account/{UserService.GetUserId()}");
            request.AddHeader("Authorization", "Bearer " + UserService.GetToken());
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);
            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("You have no past transfers.");
                    return response.Data;
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
