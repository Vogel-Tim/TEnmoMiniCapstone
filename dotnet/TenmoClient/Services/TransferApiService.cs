using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using TenmoClient.Models;

namespace TenmoClient
{
    public class TransferApiService
    {

        private readonly static string API_URL = "https://localhost:44315/transfers/";
        private readonly IRestClient client;
        private readonly ApiUser user = new ApiUser();

        public bool LoggedIn
        {
            get
            {
                return !string.IsNullOrWhiteSpace(user.Token);
            }
        }

        public TransferApiService(IRestClient restClient)
        {
            client = restClient;
        }

        public List<Transfer> GetTransfers()
        {
            RestRequest request = new RestRequest($"{API_URL}account/{user.UserId}");
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);
            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }
            else
            {
                return response.Data;
            }
        }

    }
}
