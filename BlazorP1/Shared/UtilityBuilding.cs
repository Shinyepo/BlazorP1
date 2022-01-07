using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorP1.Shared
{
    public class UtilityBuilding
    {
        [Key]
        public int Id { get; set; }
        public string BuildingName { get; set; }
        public string BuildingDescription { get; set; }
        public int MaxLevel { get; set; }
        public int BananaCostBase { get; set; }
        public int BananaCostStep { get; set; }
        public int BuildTimeBase { get; set; }
        public int BuildTimeStep { get; set; }
        public string BonusType { get; set; }
        public int BonusAmountBase { get; set; }
        public int BonusAmountStep { get; set; }

    }
}
