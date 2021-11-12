using Blazored.LocalStorage;
using BlazorP1.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorP1.Client
{
    public class CustomAuthProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public CustomAuthProvider(ILocalStorageService localStorage, HttpClient client,
            IBananaService bananaservice)
        {
            _localStorage = localStorage;
            _Client = client;
            _Bananaservice = bananaservice;
        }

        private readonly HttpClient _Client;

        public IBananaService _Bananaservice { get; }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string authToken = await _localStorage.GetItemAsStringAsync("authToken");

            var identity = new ClaimsIdentity();
            _Client.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(authToken))
            {
                try
                {

                    identity = new ClaimsIdentity(ParseClaimsFromJWT(authToken), "jwt");
                    _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.Replace("\"",""));
                    await _Bananaservice.GetBananas();
                    }
                catch (Exception)
                {
                    await _localStorage.RemoveItemAsync("authToken");
                    identity = new ClaimsIdentity();
                }

            }

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;

        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        private IEnumerable<Claim> ParseClaimsFromJWT(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            var claims = keyValuePairs.Select(x => new Claim(x.Key, x.Value.ToString()));
            return claims;
        }
    }
}

