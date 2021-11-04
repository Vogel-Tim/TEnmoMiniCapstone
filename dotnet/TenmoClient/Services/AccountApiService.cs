using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using TenmoClient.Models;

namespace TenmoClient
{
    public class AccountApiService
    {

        private readonly static string API_URL = "https://localhost:44315/accounts/";
        private readonly IRestClient client = new RestClient();
        private static ApiUser user = new ApiUser();

        public bool LoggedIn
        {
            get
            {
                return !string.IsNullOrWhiteSpace(user.Token);
            }
        }

        public AccountApiService()
        {
            
        }

        public AccountApiService(IRestClient restClient)
        {
            client = restClient;
        }

        public decimal GetBalance()
        {            
            RestRequest request = new RestRequest($"{API_URL}{user.UserId}");
            IRestResponse<Account> response = client.Get<Account>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }
            else
            {
                Account account = response.Data;
                return account.Balance;
            }
        }

    }
}
