using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorP1.Shared
{
    public class UserBuilding
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BuildingId { get; set; }
        public int BuildingLevel { get; set; }
        public bool isFinished { get; set; } = false;
        public string BuildingType { get; set; }
        public DateTime BuildFinishTime { get; set; }
        public DateTime BuildStartTime { get; set; } = DateTime.Now;
    }
}
