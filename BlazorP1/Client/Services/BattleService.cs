using BlazorP1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorP1.Client.Services
{
    public class BattleService : IBattleService
    {
        private readonly HttpClient _http;
        public BattleResult LastBattle { get; set; } = new BattleResult();
        public IList<BattleHistoryEntry> History { get; set; } = new List<BattleHistoryEntry>();

        public int SinceLastLogin { get; set; } = 0;

        public BattleService(HttpClient http)
        {
            _http = http;
        }


        public async Task<BattleResult> StartBattle(int opponentId)
        {
            var result = await _http.PostAsJsonAsync("api/battle/startbattle", opponentId);
            LastBattle = await result.Content.ReadFromJsonAsync<BattleResult>();
            return LastBattle;
        }
        
        public async Task<BattleResult> StartMission(string MissionLevel)
        {
            var result = await _http.PostAsJsonAsync("api/battle/startmission", MissionLevel);
            LastBattle = await result.Content.ReadFromJsonAsync<BattleResult>();
            return LastBattle;
        }

        public async Task GetHistory()
        {
            History = await _http.GetFromJsonAsync<BattleHistoryEntry[]>("api/user/history");
        }

        public async Task GetLastHistory()
        {
            SinceLastLogin = await _http.GetFromJsonAsync<int>("api/user/sincelastlogin");            
        }

        public async Task LastLoginUpdate()
        {
            await _http.PutAsJsonAsync("api/user/llupdate",true);
        }
    }
}
