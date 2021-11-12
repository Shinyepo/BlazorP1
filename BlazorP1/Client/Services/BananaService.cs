using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorP1.Client.Services
{
    public class BananaService : IBananaService
    {
        private readonly HttpClient _client;

        public int Bananas { get; set; } = 1000;
        public event Action OnChange;

        public BananaService(HttpClient client)
        {
            _client = client;
        }

        public async Task EatBananas(int amount)
        {
            var result = await _client.PutAsJsonAsync<int>("api/user/eatbananas", amount);
            Bananas = await result.Content.ReadFromJsonAsync<int>();
            BananasChanged();
        }

        public async Task GetBananas()
        {
            Bananas = await _client.GetFromJsonAsync<int>("api/user/getbananas");
            BananasChanged();
        }

        public async Task GrowBananas(int amount)
        {
            var result = await _client.PutAsJsonAsync<int>("api/user/addbananas", amount);
            Bananas = await result.Content.ReadFromJsonAsync<int>();
            BananasChanged();
        }

        void BananasChanged() => OnChange.Invoke();
    }
}
