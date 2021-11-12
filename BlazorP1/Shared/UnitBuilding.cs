using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorP1.Shared
{
    public class UnitBuilding
    {
        [Key]
        public int Id { get; set; }
        public string BuildingName { get; set; }        
        public Unit Unit { get; set; }
        public int UnitId { get; set; }
        public int MaxLevel { get; set; }
        public int UnitLimitStep { get; set; }
        public int UnitLimitBase { get; set; }
        public int BananaCostBase { get; set; }
        public int BananaCostStep { get; set; }
        public int BuildTimeBase { get; set; }
        public int BuildTimeStep { get; set; }
        public int TaskTimeBase { get; set; }
        public int TaskTimeStep { get; set; }
    }
}
