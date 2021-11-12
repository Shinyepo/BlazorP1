using BlazorP1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorP1.Client.Services
{
    public interface IBuildingService
    {
        event Action OnChange;

        IList<UserBuilding> UserBuildings { get; set; }
        IList<UnitBuilding> UnitBuildings { get; set; }
        IList<ProductionBuilding> ProductionBuildings { get; set; }
        IList<UtilityBuilding> UtilityBuildings { get; set; }


        Task GetBuildings();
        Task StartBuilding(int BuildingId, string BuildingType);
        Task CancelBuilding(int BuildingId, string BuildingType);
        Task FinishBuilding(int BuildingId, string BuildingType);
        Task EndTask(int BuildingId, string BuildingType);
        Task<int> GetMaxBananas(int BuildingId, int Level);
        Task<UserBuildingTask> GetTask(int BuildingId);

        Task<int> GetReadyBuildings();
    }
}
