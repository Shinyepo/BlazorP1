﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorP1.Shared
{
    public class Unit
    {
        [Key]
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public UnitBuilding UnitBuilding { get; set; }
        public string Title { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public int HitPoints { get; set; } = 100;
        public int BananaCost { get; set; }

    }
}
