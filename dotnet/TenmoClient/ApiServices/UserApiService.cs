using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Models;
using RestSharp;
using TenmoClient.Services;
using System.Threading;

namespace TenmoClient.ApiServices
{
    public class UserApiService
    {
        private readonly static string API_URL = "https://localhost:44315/users/";
        private readonly IRestClient client = new RestClient();
        private static ApiUser user = new ApiUser();

        public UserApiService()
        {

        }

        public UserApiService(IRestClient restClient)
        {
            client = restClient;
        }

        public List<ApiUser> GetUsers()
        {
            RestRequest request = new RestRequest(API_URL);
            request.AddHeader("Authorization", "Bearer " + UserService.GetToken());
            IRestResponse<List<ApiUser>> response = client.Get<List<ApiUser>>(request);
            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                Console.WriteLine("An error occured fetching Users");
                Thread.Sleep(2000);
                return new List<ApiUser>();
            }
            else
            {
                return response.Data;
            }
        }
    }
}
