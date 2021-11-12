using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorP1.Shared
{
    public static class StaticDetails
    {
        public enum BuildingType { Unit = 0, Production = 1, Utility = 2 };
        public const string UnitBuilding = "Unit";
        public const string ProductionBuilding = "Production";
        public const string UtilityBuilding = "Utility";
        public const string RecruitUtilityBuilding = "Training Camp";
        public const string HealCostUtilityBuilding = "Shaman's Hut";
        public const string MissionEasy = "Easy";
        public const string MissionMedium = "Medium";
        public const string MissionHard = "Hard";
        public const string MissionImpossible = "Impossible";
        public const int HealCost = 40;
    }
}
