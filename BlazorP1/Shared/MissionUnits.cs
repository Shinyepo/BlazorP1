using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorP1.Shared
{
    public class MissionUnits
    {
        [Key]
        public int Id { get; set; }
        public int MissionLevelId { get; set; }
        public MissionLevel MissionLevel { get; set; } 
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        public int HitPoints { get; set; }
    }
}