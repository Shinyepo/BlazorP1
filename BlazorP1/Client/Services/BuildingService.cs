using BlazorP1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net;
using Blazored.Toast.Services;

namespace BlazorP1.Client.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly HttpClient _http;
        private readonly IToastService _tService;
        private readonly IBananaService _bananaService;

        public event Action OnChange;

        public IList<UserBuilding> UserBuildings { get; set; }
        public IList<UnitBuilding> UnitBuildings { get; set; }
        public IList<ProductionBuilding> ProductionBuildings { get; set; }
        public IList<UtilityBuilding> UtilityBuildings { get; set; }

        public BuildingService(HttpClient http, IToastService tService, IBananaService bananaService)
        {
            _http = http;
            _tService = tService;
            _bananaService = bananaService;
        }

        void UserBuildingsChanged() => OnChange.Invoke();

        public async Task FinishBuilding(int BuildingId, string BuildingType)
        {
            var request = new BuildingRequest
            {
                BuildingId = BuildingId,
                BuildingType = BuildingType
            };
            var result = await _http.PutAsJsonAsync<BuildingRequest>("api/building", request);
            var response = await result.Content.ReadAsStringAsync();

            if (result.StatusCode == HttpStatusCode.OK)
            {
                _tService.ShowSuccess(response);
                await GetBuildings();
                UserBuildingsChanged();
            }
            else
            {
                _tService.ShowError(response);
            }

        }

        public async Task<int> GetReadyBuildings()
        {
            var result = await _http.GetFromJsonAsync<int>("api/building/getreadybuildings");
            return result;
        }

        public async Task GetBuildings()
        {
            var BuildingViewModel = await _http.GetFromJsonAsync<BuildingViewModel>("api/building");
            UnitBuildings = BuildingViewModel.UnitBuildings;
            ProductionBuildings = BuildingViewModel.ProductionBuildings;
            UtilityBuildings = BuildingViewModel.UtilityBuildings;
            UserBuildings = BuildingViewModel.UserBuildings;
        }

        public async Task StartBuilding(int BuildingId, string BuildingType)
        {
            
            var request = new BuildingRequest
            {
                BuildingId = BuildingId,
                BuildingType = BuildingType
            };
            var result = await _http.PostAsJsonAsync<BuildingRequest>("api/building", request);
            var response = await result.Content.ReadAsStringAsync();

            if (result.StatusCode == HttpStatusCode.OK)
            {
                _tService.ShowSuccess(response);
                await _bananaService.GetBananas();
                await GetBuildings();
                UserBuildingsChanged();
            }
            else
            {
                _tService.ShowError(response);
            }

        }

        public async Task CancelBuilding(int BuildingId, string BuildingType)
        {
            var request = new BuildingRequest
            {
                BuildingId = BuildingId,
                BuildingType = BuildingType
            };
            var result = await _http.PutAsJsonAsync<BuildingRequest>("api/building/cancel", request);
            var response = await result.Content.ReadAsStringAsync();

            if (result.StatusCode == HttpStatusCode.OK)
            {
                _tService.ShowSuccess(response);
                await _bananaService.GetBananas();
                await GetBuildings();
                UserBuildingsChanged();
            }
            else
            {
                _tService.ShowError(response);
            }
        }

        public async Task EndTask(int BuildingId, string BuildingType)
        {
            var request = new BuildingRequest
            {
                BuildingId = BuildingId,
                BuildingType = BuildingType
            };
            var result = await _http.PutAsJsonAsync<BuildingRequest>("api/building/endtask", request);
            var response = await result.Content.ReadAsStringAsync();

            if (result.StatusCode == HttpStatusCode.OK)
            {
                _tService.ShowSuccess(response);
                await _bananaService.GetBananas();
                await GetBuildings();
                UserBuildingsChanged();
            }
            else
            {
                _tService.ShowError(response);
            }
        }

        public async Task<int> GetMaxBananas(int BuildingId, int Level)
        {
            var result = await _http.GetFromJsonAsync<int>($"api/building/getmaxbananas/{BuildingId}/{Level}");
            return result;
        }
        public async Task<UserBuildingTask> GetTask(int BuildingId)
        {
            var result = await _http.GetFromJsonAsync<UserBuildingTask>($"api/building/gettask/{BuildingId}");
            return result;
        }
    }
}
