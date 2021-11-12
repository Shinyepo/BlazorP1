using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorP1.Shared
{
    public class BuildingViewModel
    {
        public List<UserBuilding> UserBuildings { get; set; }
        public List<UnitBuilding> UnitBuildings { get; set; }
        public List<ProductionBuilding> ProductionBuildings { get; set; }
        public List<UtilityBuilding> UtilityBuildings { get; set; }
    }
}
