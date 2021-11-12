using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorP1.Shared
{
    public class UserBuildingTask
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserBuildingId { get; set; }
        public UserBuilding UserBuilding { get; set; }
        public DateTime TaskEndTime { get; set; }
        public DateTime LastTaskEndTime { get; set; }
        public string TaskType { get; set; }
    }
}
