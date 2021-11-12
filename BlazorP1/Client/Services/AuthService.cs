using BlazorP1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorP1.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpclient;

        public AuthService(HttpClient httpclient)
        {
            _httpclient = httpclient;
        }
        public async Task<ServiceResponse<int>> Register(UserRegister request)
        {
            var result = await _httpclient.PostAsJsonAsync("api/auth/register", request);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }
        
        public async Task<ServiceResponse<string>> Login(UserLogin request)
        {
            var result = await _httpclient.PostAsJsonAsync("api/auth/login", request);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

    }
}
