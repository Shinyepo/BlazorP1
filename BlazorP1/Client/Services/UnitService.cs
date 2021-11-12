using Blazored.Toast.Services;
using BlazorP1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorP1.Client.Services
{
    public class UnitService : IUnitService
    {
        private readonly IToastService _tService;
        private readonly HttpClient _HttpClient;
        private readonly IBananaService _BananaService;

        public UnitService(IToastService tService, HttpClient httpclient, IBananaService bananaService)
        {
            _tService = tService;
            _HttpClient = httpclient;
            _BananaService = bananaService;
        }

        public IList<Unit> Units { get; set; } = new List<Unit>();
        public IList<UserUnit> MyUnits { get; set; } = new List<UserUnit>();

        public async Task AddUnit(int unitId)
        {
            var unit = Units.First(x => x.Id == unitId);
            var response = await _HttpClient.PostAsJsonAsync<int>("api/UserUnit", unitId);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var msg = await response.Content.ReadAsStringAsync();
                _tService.ShowError(msg);
            }
            else
            {
                await _BananaService.GetBananas();
                _tService.ShowSuccess($"You just bought {unit.Title}!", "Unit built!");
            }
        }

        public async Task LoadUnitsAsync()
        {
            if (Units.Count() == 0)
            {
                Units = await _HttpClient.GetFromJsonAsync<IList<Unit>>("api/Unit");
            }
        }

        public async Task LoadUserUnitsAsync()
        {
            MyUnits = await _HttpClient.GetFromJsonAsync<IList<UserUnit>>("api/userunit");
        }

        public async Task ReviveArmy()
        {
            var result = await _HttpClient.PostAsJsonAsync<string>("api/userunit/revive", null);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                _tService.ShowError(await result.Content.ReadAsStringAsync());
            }
            else
            {
                _tService.ShowSuccess(await result.Content.ReadAsStringAsync());
            }

            await LoadUnitsAsync();
            await _BananaService.GetBananas();
        }
    }
}
