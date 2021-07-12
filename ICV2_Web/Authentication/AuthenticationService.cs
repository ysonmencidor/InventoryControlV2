using Blazored.LocalStorage;
using DataAccessLibrary.Models;
using ICV2_Web.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace ICV2_Web.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient client;
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly ILocalStorageService localStorage;

        public AuthenticationService(HttpClient client,
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorage)
        {
            this.client = client;
            this.authenticationStateProvider = authenticationStateProvider;
            this.localStorage = localStorage;
        }

        public async Task<AuthenticatedUserModel> Login(UserModel userForAuthentication)
        {

            //NOT WORKING
            //var data = new FormUrlEncodedContent(new[]
            //{
            //    new KeyValuePair<string,string>("username", userForAuthentication.Username),
            //    new KeyValuePair<string,string>("password", userForAuthentication.Password)
            //});


            //"application/x-www-form-urlencoded" convert to FormUrlEncodedContent
            var dict = new Dictionary<string, string>();
            dict.Add("username", userForAuthentication.Username);
            dict.Add("password", userForAuthentication.Password);

            var authResult = await client.PostAsync("Token", new FormUrlEncodedContent(dict));

            var authContent = await authResult.Content.ReadAsStringAsync();

            if (authResult.IsSuccessStatusCode)
            {

                var result = JsonSerializer.Deserialize<AuthenticatedUserModel>(
                    authContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                await localStorage.SetItemAsync("authToken", result.Access_Token);


                ((CustomAuthStateProvider)authenticationStateProvider).NotifyUserAuthentication(result.Access_Token);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Access_Token);

                return result;
            }
            return new AuthenticatedUserModel();
        }
        public async Task LogOut()
        {
            await localStorage.RemoveItemAsync("authToken");
            ((CustomAuthStateProvider)authenticationStateProvider).NotifyUserLogOut();
            client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
