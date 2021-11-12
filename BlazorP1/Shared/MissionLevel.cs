using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorP1.Shared
{
    public class MissionLevel
    {
        [Key]
        public int Id { get; set; }
        public string MissionName { get; set; }
        public int Battles { get; set; }
        public int Wins { get; set; }
        public int Defeats { get; set; }

        public List<MissionUnits> Units { get; set; }

    }
}
