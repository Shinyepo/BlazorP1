using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorP1.Shared
{
    public class ProductionBuilding
    {
        [Key]
        public int Id { get; set; }
        public string BuildingName { get; set; }
        public int MaxLevel { get; set; }
        public int BananaCostBase { get; set; }
        public int BananaCostStep { get; set; }
        public int BuildTimeBase { get; set; }
        public int BuildTimeStep { get; set; }
        public int BananaCountBase { get; set; }
        public int BananaCountStep { get; set; }
        public int TaskTimeBase { get; set; }
        public int TaskTimeStep { get; set; }
    }
}
