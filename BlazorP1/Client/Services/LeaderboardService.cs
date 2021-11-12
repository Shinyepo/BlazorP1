using BlazorP1.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorP1.Client.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly HttpClient _http;
        private readonly IBananaService _BananaService;
        private readonly AuthenticationStateProvider _StateProvider;

        public LeaderboardService(HttpClient Http, IBananaService BananaService, AuthenticationStateProvider StateProvider)
        {
            _http = Http;
            _BananaService = BananaService;
            _StateProvider = StateProvider;
        }
        public IList<UserStatistic> Leaderboard { get;  set; }
        public int MyUserId { get; set; }

        public async Task GetLeaderboard()
        {
            var state = await _StateProvider.GetAuthenticationStateAsync();
            MyUserId = int.Parse(state.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            Leaderboard = await _http.GetFromJsonAsync<IList<UserStatistic>>("api/user/leaderboard");
        }

        public async Task<int> GetMyRank()
        {
            var respone = await _http.GetFromJsonAsync<int>("api/user/myrank");
            return respone;
        }
    }
}
