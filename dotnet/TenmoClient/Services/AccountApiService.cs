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

        public Account GetAccount()
        {
            string token = UserService.GetToken();
            RestRequest request = new RestRequest($"{API_URL}{UserService.GetUserId()}");
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse<Account> response = client.Get<Account>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                throw new Exception();
            }
            else
            {
                return response.Data;
            }
        }

        public ApiUser GetUserAccount(int accountId)
        {
            RestRequest request = new RestRequest($"{API_URL}account/{accountId}");
            request.AddHeader("Authorization", "Bearer " + UserService.GetToken());
            IRestResponse<ApiUser> response = client.Get<ApiUser>(request);

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
