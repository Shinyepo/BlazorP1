using BlazorP1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
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
            request.Email = Regex.Replace(request.Email, @"\s", "");
            request.Username = Regex.Replace(request.Username, @"\s", "");
            var result = await _httpclient.PostAsJsonAsync("api/auth/register", request);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }
        
        public async Task<ServiceResponse<string>> Login(UserLogin request)
        {
            request.Email = Regex.Replace(request.Email, @"\s", "");
            var result = await _httpclient.PostAsJsonAsync("api/auth/login", request);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> ChangePassword(PasswordChangeForm request)
        {
            var result = await _httpclient.PostAsJsonAsync("api/auth/changepassword", request);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> RequestPasswordChange(string email)
        {
            email = Regex.Replace(email, @"\s", "");

            var result = await _httpclient.PostAsJsonAsync("api/auth/requestpasswordchange", email);
            
            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

    }
}
